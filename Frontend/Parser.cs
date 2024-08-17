using Ds.Frontend.Ast;

namespace Ds.Frontend {
    public class Parser {
        public Lexer lexer = new();
        private List<Token> toks = new();
        private Token At() => toks[0];
        private Token Eat() {
            Token tok = At();
            toks.RemoveAt(0);
            return tok;
        }
        private Token Expect(TokenType type, string Error) {
            if (At().Type != type) {
                throw new Exception(Error);
            }
            return Eat();
        }
        private bool Not_eof() => At().Type != TokenType.EOF;
        public ProgramNode ProduceAst(string src) {
            ProgramNode program = new();
            toks = lexer.Tokenize(src);

            while (Not_eof()) {
                program.Body.Add(Parse_stmt());
            }

            return program;
        }

        private IStmt Parse_stmt() => At().Type switch
        {
            TokenType.LetKeyword or TokenType.ConstKeyword => Parse_var_dec(),
            TokenType.FunctionKeyword => Parse_fun_dec(),
            TokenType.ReturnKeyword => Parse_ret_stmt(),
            _ => Parse_expr(),
        };

        private ReturnStmt Parse_ret_stmt() {
            Eat();
            IExpr expr = Parse_expr().Expr;
            Expect(TokenType.Semicolon,"Expected semicolon after the return stmt.");
            return new ReturnStmt(expr);
        }

        private FunctionDeclaration Parse_fun_dec() {
            Eat();
            FunctionDeclaration stmt = new(Expect(TokenType.Indetifer,"Expected identifier after the function keyword.").Value);
            List<IExpr> args = Parse_args();
            stmt.Block = Parse_block();
            foreach (IExpr arg in args){
                if (arg.Kind != NodeType.Identifier) {
                    throw new Exception("Inside function declaration expected parameter to be of type string.");
                }

                stmt.Args.Add(((Identifier)arg).Value);
            }
            return stmt;
        }

        private VarDeclaration Parse_var_dec() {
            VarDeclaration declaration = new()
            {
                Constant = Eat().Type == TokenType.ConstKeyword
            };
            Parse_multiple(TokenType.Indetifer,"Expected identifier after the let, const or comma (\",\").",declaration.Identifiers);

            if (At().Type == TokenType.Equals) {
                Eat();
                do {
                    ExprStmt expr = Parse_expr();
                    declaration.Vals.Add(expr.Expr);
                    if (At().Type == TokenType.Comma) Eat();
                } while (At().Type is not TokenType.Comma and not TokenType.Semicolon);
            }
            
            if (declaration.Vals.Count == 0 && declaration.Constant) {
                throw new Exception("Constant declarations must be initilized.");
            }

            Expect(TokenType.Semicolon,"Variable declarations must end with semicolon.");

            return declaration;
        }

        private List<IExpr> Parse_args() {
            Expect(TokenType.OpenParen,"Expected open parenthesis");
            List<IExpr> args = At().Type == TokenType.CloseParen 
                ? new()
                : Parse_arguments_list();

            Expect(TokenType.CloseParen,"Missing closing parenthesis inside arguments list");
            return args;
        }

        private List<IExpr> Parse_arguments_list() {
            List<IExpr> args = new();

            do {
                args.Add(Parse_assignment());
                if (At().Type == TokenType.Comma) Eat();
            } while (Not_eof() && At().Type == TokenType.Comma);

            return args;
        }

        private IExpr Parse_paren() {
            Eat();
            IExpr expr = Parse_expr().Expr;
            Expect(TokenType.CloseParen,"Expected close paren.");
            return expr;
        }

        private List<string> Parse_multiple(TokenType type, string err, List<string> list) {
            do {
                Token tok = Expect(type,err);
                list.Add(tok.Value);
                if (At().Type == TokenType.Comma) Eat();
                if (At().Type == TokenType.Semicolon) break;
            } while (At().Type is not TokenType.Comma);
            return list;
        }

        private ExprStmt Parse_expr() => new(Parse_assignment());

        private IExpr Parse_assignment() {
            IExpr left = Parse_object();

            if (At().Type == TokenType.Equals) {
                Eat();
                IExpr right = Parse_assignment();
                Expect(TokenType.Semicolon,"Variable assignment statment must end with semicolon.");
                left = new AssignmentExpr(left,right);
            }

            return left;
        }

        private BlockStmt Parse_block() {
            BlockStmt block = new();
            Expect(TokenType.OpenBrace,"Expected body following declaration.");

            while (Not_eof() && At().Type != TokenType.CloseBrace) {
                block.Stmts.Add(Parse_stmt());
            }

            Expect(TokenType.CloseBrace,"Expected closebrace after the body.");
            return block;
        }

        private IExpr Parse_object() {
            if (At().Type != TokenType.OpenBrace) return Parse_array();

            Eat();
            List<Propery> props = new();

            while (Not_eof() && At().Type != TokenType.CloseBrace) {
                string key = Expect(TokenType.Indetifer,"Object literal key exprected").Value;

                if (At().Type is TokenType.Comma or TokenType.CloseBrace) {
                    if (At().Type == TokenType.Comma) Eat();
                    props.Add(new Propery(key));
                    continue;
                }
                Expect(TokenType.Colon, "Missing colon flowing identifier in ObjectExpr");
                IExpr val = Parse_expr().Expr;

                props.Add(new Propery(key,val));
                if (At().Type != TokenType.CloseBrace) {
                    Expect(TokenType.Comma, "Expected Comma or Closing Brace following Property");
                }                
            }

            Expect(TokenType.CloseBrace,"Object literal missing closing brace.");
            return new ObjectLiteralExpr(props);
        }

        private IExpr Parse_array() {
            if (At().Type != TokenType.OpenBracket) return Parse_additive();
            Eat();
            List<IExpr> vals = new();

            while (Not_eof() && At().Type != TokenType.CloseBracket) {
                vals.Add(Parse_expr().Expr);

                if (At().Type != TokenType.CloseBracket) {
                    Expect(TokenType.Comma, "Comma or closing bracket expected after \"value\" in array.");
                }
            }

            Expect(TokenType.CloseBracket,"Closing Bracket expected at the end of \"Array\" expression.");
            return new ArrayLiteralExpr(vals);
        }

        private IExpr Parse_additive() {
            IExpr left = Parse_multiplicitave();

            while (At().Value is "+" or "-") {
                string op = Eat().Value;
                IExpr right = Parse_multiplicitave();
                left = new BinExpr(left,op,right);
            }

            return left;
        }
        private IExpr Parse_multiplicitave() {
            IExpr left = Parse_call_member();

            while (At().Value is "*" or "/" or "^") {
                string op = Eat().Value;
                IExpr right = Parse_call_member();
                left = new BinExpr(left,op,right);
            }

            return left;
        }

        private IExpr Parse_call_member() {
            IExpr member = Parse_member();

            if (At().Type == TokenType.OpenParen) {
                member = Parse_call_expr(member);
            }

            return member;
        }
        
        private IExpr Parse_call_expr(IExpr caller) {
            IExpr call_expr = new CallExpr(
                caller,
                Parse_args()
            );

            if (At().Type == TokenType.OpenParen) {
                call_expr = Parse_call_expr(call_expr);
            }

            return call_expr;
        }

        private IExpr Parse_member() {
            IExpr obj = Parse_primary();

            while (At().Type is TokenType.Dot or TokenType.OpenBracket) {
                Token op = Eat();
                IExpr prop;
                bool computed;

                if (op.Type == TokenType.Dot) {
                    computed = false;
                    prop = Parse_primary();

                    if (prop.Kind != NodeType.Identifier) {
                        throw new Exception("Can\'t use dot operator without right hand side being a identifier");
                    }
                } else {
                    computed = true;
                    prop = Parse_expr().Expr;
                    Expect(TokenType.CloseBracket, "Missing closing bracket in computed value.");
                }
                obj = new MemberExpr(obj,prop,computed);
            }

            return obj;
        }

        private IExpr Parse_primary() => At().Type switch
        {
            TokenType.OpenParen => Parse_paren(),
            TokenType.Number => new NumericLiteral(float.Parse(Eat().Value)),
            TokenType.Indetifer => new Identifier(Eat().Value),
            _ => throw new Exception("Unexpected token found during parsing. " + At().Value),
        };
    }
}