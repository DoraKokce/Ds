namespace Ds.Frontend {
    public enum TokenType {
        Indetifer,
        Number,

        BinOp,

        LetKeyword,
        ConstKeyword,
        FunctionKeyword,
        ReturnKeyword,

        Equals,
        OpenParen,CloseParen,
        OpenBrace,CloseBrace,
        OpenBracket,CloseBracket,
        Dot,
        Comma,
        Colon,
        Semicolon,

        EOF
    }

    public class Token {
        public TokenType Type { get; }
        public string Value { get; }
        public Dictionary<string,int> pos = new();
        public Token(TokenType type, string val) {
            Type = type;
            Value = val;
        }
    }

    public class Lexer {

        public Dictionary<string,TokenType> keywords = new() {
            {"let",TokenType.LetKeyword},
            {"const",TokenType.ConstKeyword},
            {"function",TokenType.FunctionKeyword},
            {"return",TokenType.ReturnKeyword},
        };
        public Dictionary<char,TokenType> symbols = new() {
            {'+',TokenType.BinOp},
            {'-',TokenType.BinOp},
            {'*',TokenType.BinOp},
            {'/',TokenType.BinOp},
            {'^',TokenType.BinOp},
            {'=',TokenType.Equals},
            {'(',TokenType.OpenParen},
            {')',TokenType.CloseParen},
            {'{',TokenType.OpenBrace},
            {'}',TokenType.CloseBrace},
            {'[',TokenType.OpenBracket},
            {']',TokenType.CloseBracket},
            {'.',TokenType.Dot},
            {',',TokenType.Comma},
            {':',TokenType.Colon},
            {';',TokenType.Semicolon},
        };

        public List<Token> Tokenize(string _src) {
            List<Token> tokens = new();
            string src = _src;
            while (src.Length > 0) {
                if (char.IsLetter(src[0])) {
                    string ident = "";

                    while (src.Length > 0 && char.IsLetterOrDigit(src[0])) {
                        ident += src[0];
                        src = src[1..];
                    }

                    if (keywords.ContainsKey(ident)) {
                        tokens.Add(new Token(keywords[ident],ident));
                        continue;
                    }

                    tokens.Add(new Token(TokenType.Indetifer,ident));
                }
                else if (char.IsNumber(src[0])) {
                    string num = "";
                    bool period = false;

                    while (src.Length > 0) {
                        if (char.IsNumber(src[0]) || (!period && src[0] == '.')) {
                            num += src[0];
                            src = src[1..];
                        } else {
                            break;
                        }
                    }

                    tokens.Add(new Token(TokenType.Number,num));
                }
                else if (symbols.ContainsKey(src[0])) {
                    tokens.Add(new Token(symbols[src[0]],src[0].ToString()));
                    src = src[1..];
                }
                else if (char.IsWhiteSpace(src[0])) {
                    src = src[1..];
                }
                else {
                    throw new Exception($"Unexpected character found in source: {src[0]}");
                }
            }

            tokens.Add(new Token(TokenType.EOF,"EndOfFile"));

            return tokens;
        }
    }
}