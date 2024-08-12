using Ds.Frontend.Ast;
using Ds.Runtime.Values;

namespace Ds.Runtime {
    public class Interpreter {

        public IRuntimeVal Eval(IStmt ast, DSEnvironment env) => ast.Kind switch
        {
            NodeType.Program => Eval_prog((ProgramNode)ast, env),
            NodeType.VarDeclaration => Eval_var((VarDeclaration)ast, env),
            NodeType.FunctionDeclaration => Eval_fn((FunctionDeclaration)ast, env),
            NodeType.ReturnStmt => Eval_ret((ReturnStmt)ast,env),
            _ => Eval_expr(((ExprStmt)ast).Expr, env),
        };

        private IRuntimeVal Eval_prog(ProgramNode program, DSEnvironment env) {
            IRuntimeVal last_evaluated = new NullVal();

            foreach (IStmt statement in program.Body) {
                last_evaluated = Eval(statement,env);
            }

            return last_evaluated;
        }

        private IRuntimeVal Eval_fn(FunctionDeclaration fun, DSEnvironment env) {
            FunctionValue fn = new(fun.Name, env, fun.Block)
            {
                Args = fun.Args,
            };
            return env.DeclareVar(fun.Name,fn,true);
        }

        private IRuntimeVal Eval_ret(ReturnStmt ret, DSEnvironment env) {
            if (!env.isFuncEnv) {
                throw new Exception("Return stmt must be on a function body.");
            }
            return new ReturnVal(Eval_expr(ret.Return,env));
        }

        private IRuntimeVal Eval_var(VarDeclaration var, DSEnvironment env) {
            IRuntimeVal last = new NullVal();
            int c = 0;
            foreach (string name in var.Identifiers) {
                last = env.DeclareVar(name,Eval_expr(var.Vals[c],env),var.Constant);
                c++;
            }
            return last;
        }

        private IRuntimeVal Eval_expr(IExpr expr, DSEnvironment env) => expr.Kind switch {
            NodeType.ParenthesizedExpr => Eval_expr(((ParenthesizedExpr)expr).Expr,env),
            NodeType.AssignmentExpr => Eval_assignment((AssignmentExpr)expr,env),
            NodeType.BinExpr => Eval_bin((BinExpr)expr, env),
            NodeType.NumericLiteral => new NumberVal(((NumericLiteral)expr).Value),
            NodeType.Identifier => env.LookupVar(((Identifier)expr).Value),
            NodeType.ObjectLiteralExpr => Eval_object((ObjectLiteralExpr)expr,env),
            NodeType.ArrayLiteralExpr => Eval_array((ArrayLiteralExpr)expr,env),
            NodeType.MemberExpr => Eval_member(env,expr:(MemberExpr)expr),
            NodeType.CallExpr => Eval_call((CallExpr)expr,env),
            _ => throw new Exception("Ast node hasn't setup for interpretion. " + expr.Kind),
        };

        private IRuntimeVal Eval_assignment(AssignmentExpr expr,DSEnvironment env) {
            if (expr.Left.Kind == NodeType.MemberExpr) return Eval_member(env,expr);
            if (expr.Left.Kind != NodeType.Identifier) throw new Exception("Expected identifier in assignment expr.");

            return env.AssignVar(((Identifier)expr.Left).Value, Eval_expr(expr.Right,env));
        }

        private IRuntimeVal Eval_call(CallExpr expr, DSEnvironment env) {
            List<IRuntimeVal> args = new();
            foreach (IExpr arg in expr.Args) {
                args.Add(Eval_expr(arg,env));
            }
            IRuntimeVal fn = Eval_expr(expr.Caller,env);
            if (fn.Type == RuntimeType.NativeFn) {
                IRuntimeVal res = ((NativeFnVal)fn).Call(args,env);
                return res;
            } else if (fn.Type == RuntimeType.FnVal) {
                FunctionValue fun = (FunctionValue)fn;
                DSEnvironment scope = new(fun.Env)
                {
                    isFuncEnv = true
                };

                if (args.Count != fun.Args.Count) {
                    throw new Exception("Expected " + fun.Args.Count.ToString() + " args. Got " + args.Count);
                }

                for (int i = 0; i < fun.Args.Count; i++) {
                    scope.DeclareVar(fun.Args[i],args[i],true);
                }

                IRuntimeVal temp = new NullVal();
                foreach (IStmt stmt in fun.Body.Stmts) {
                    temp = Eval(stmt,scope);
                    if (temp.Type == RuntimeType.Return) {
                        return ((ReturnVal)temp).Ret;
                    }
                }

                return new NullVal();
            }

            throw new Exception("Can't call value that is not a function.");
        }

        private IRuntimeVal Eval_member(DSEnvironment env, AssignmentExpr? node = null, MemberExpr? expr = null) {
            if (expr != null) return env.LookupOrMutObj(expr);
            if (node != null) return env.LookupOrMutObj((MemberExpr)node.Left, Eval_expr(node.Right, env));

            throw new Exception("Evaluating a member expression is not possible without a member or assignment expression.");
        }

        private IRuntimeVal Eval_bin(BinExpr expr, DSEnvironment env) {
            IRuntimeVal left = Eval_expr(expr.Left,env);
            IRuntimeVal right = Eval_expr(expr.Right,env);
            if (left.Type != RuntimeType.Number || right.Type != RuntimeType.Number) {
                throw new Exception("Expected numbers in binary expr.");
            }
            return new NumberVal(expr.Op switch {
                "+" => ((NumberVal)left).Value + ((NumberVal)right).Value,
                "-" => ((NumberVal)left).Value - ((NumberVal)right).Value,
                "*" => ((NumberVal)left).Value * ((NumberVal)right).Value,
                "/" => ((NumberVal)left).Value / ((NumberVal)right).Value,
                "^" => Math.Pow(((NumberVal)left).Value,((NumberVal)right).Value),
                _ => throw new Exception("Unexpected operator"),
            });
        }
        
        private IRuntimeVal Eval_object(ObjectLiteralExpr expr, DSEnvironment env) {
            ObjectVal objVal = new();

            foreach (Propery prop in expr.Properties) {
                IRuntimeVal val = (prop.Value == null) ? env.LookupVar(prop.Key) : Eval_expr(prop.Value,env);

                objVal.properties[prop.Key] = val;
            }

            return objVal;
        }

        private IRuntimeVal Eval_array(ArrayLiteralExpr expr, DSEnvironment env) {
            ArrayVal val = new();

            foreach (IExpr value in expr.Vals) {
                val.Vals.Add(Eval_expr(value,env));
            }

            return val;
        }
    }
}