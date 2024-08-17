using Ds.Frontend.Ast;
using Ds.Runtime.Values;

namespace Ds.Runtime {
    public class DSEnvironment {
        private DSEnvironment? Parent;
        public Dictionary<string, IRuntimeVal> Vars = new();
        public List<string> Consts = new();
        public Dictionary<string, RuntimeType> Types = new();
        public bool isFuncEnv = false;

        public DSEnvironment(DSEnvironment? parent = null) {
            Parent = parent;
        }

        public IRuntimeVal DeclareVar(string name, IRuntimeVal val, bool constant = false) {
            if (Vars.ContainsKey(name)) {
                throw new Exception($"Cannot declare variable {name}. As it already is defined.");
            }
            if (constant) Consts.Add(name);
            Vars[name] = val;
            Types[name] = val.Type;

            return val;
        }

        public IRuntimeVal AssignVar(string name, IRuntimeVal val) {
            DSEnvironment env = Resolve(name);
            if (env.Consts.Contains(name)) throw new Exception($"Cannot reasign to variable {name} as it was declared constant.");
            if (env.Types[name] != val.Type && val.Type != RuntimeType.Null) throw new Exception($"Type mismatch for variable '{name}'. Expected {Types[name]}, got {val.Type}.");
            env.Vars[name] = val;
            env.Types[name] = val.Type;

            return val;
        }

        public IRuntimeVal LookupVar(string name) {
            DSEnvironment env = Resolve(name);

            if (!env.Vars.ContainsKey(name)) {
                throw new Exception($"Undeclared identifier '{name}'.");
            }

            return env.Vars[name];
        }

        public IRuntimeVal LookupOrMutObj(MemberExpr expr, IRuntimeVal? val = null, Identifier? prop = null) {
            IRuntimeVal pastVal;
            if (expr.Obj.Kind == NodeType.MemberExpr) {
                pastVal = LookupOrMutObj((MemberExpr)expr.Obj,prop:(Identifier)((MemberExpr)expr.Obj).Prop);
            } else {
                string varname = ((Identifier)expr.Obj).Value;
                DSEnvironment env = Resolve(varname);

                pastVal = env.Vars[varname];
            }

            switch (pastVal.Type) {
                case RuntimeType.Object:
                    string currProp = ((Identifier)expr.Prop).Value;
                    string property = prop != null ? prop.Value : currProp;
                    if (val != null) ((ObjectVal)pastVal).properties[property] = val;
                    if (currProp != null) pastVal = ((ObjectVal)pastVal).properties.ContainsKey(currProp) 
                        ? ((ObjectVal)pastVal).properties[currProp]
                        : throw new Exception($"Property '{currProp}' does not exist on type 'object'.");

                    return pastVal;
                default:
                    throw new Exception($"Cannot lookup or mutate type: {pastVal.Type}");
            }
        }

        public DSEnvironment Resolve(string name) {
            if (Vars.ContainsKey(name)) {
                return this;
            }

            if (Parent == null) {
                throw new Exception($"Cannot resolve \'{name}\' as it does not exist.");
            }

            return Parent.Resolve(name);
        }

        public DSEnvironment CreateGlobalEnv() {
            DeclareVar("print",new NativeFnVal((List<IRuntimeVal> args,DSEnvironment env) => {
                foreach (IRuntimeVal arg in args) {
                    if (arg.Type == RuntimeType.Number) {
                        Console.WriteLine(((NumberVal)arg).Value);
                    } else {
                        Console.WriteLine(arg);
                    }
                }
                return new NullVal();
            }));
            DeclareVar("true",new BoolVal(true),true);
            DeclareVar("false",new BoolVal(false),true);
            return this;
        }
    }
}