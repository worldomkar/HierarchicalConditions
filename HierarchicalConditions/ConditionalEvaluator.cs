/// <summary>
/// 
/// </summary>
namespace HierarchicalConditions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ConditionalEvaluator simply replaces if/else with organised constructs.
    /// Code becomes readable and maintainable.
    /// </summary>
    public class ConditionalEvaluator
    {
        /// <summary>
        /// Basic condition types
        /// </summary>
        public enum Condition
        {
            And = 0,
            Or = 1,
            Not = 2
        }

        List<ConditionalEvaluator> ce = new List<ConditionalEvaluator>();

        public ConditionalEvaluator Also { get; private set; }
        Condition conditionType = Condition.And;
        bool condition = false;
        Func<int> truthyFunc = null;
        Func<int> falsyFunc = null;

        public ConditionalEvaluator(bool condition, Func<int> truthyFunc = null, Func<int> falsyFunc = null, Condition conditionType = Condition.And)
        {
            Also = this;
            this.condition = condition;
            this.truthyFunc = truthyFunc;
            this.falsyFunc = falsyFunc;
            this.conditionType = conditionType;
        }

        public ConditionalEvaluator And(bool condition, Func<int> truthyFunc = null, Func<int> falsyFunc = null)
        {
            ConditionalEvaluator tAnd =
                    new ConditionalEvaluator(condition, truthyFunc, falsyFunc, Condition.And);
            tAnd.Also = this;
            ce.Add(tAnd);
            return tAnd;
        }

        public ConditionalEvaluator Or(bool condition, Func<int> truthyFunc = null, Func<int> falsyFunc = null)
        {
            ConditionalEvaluator or =
                new ConditionalEvaluator(condition, truthyFunc, falsyFunc, Condition.Or);
            or.Also = this;
            ce.Add(or);
            return or;
        }

        public ConditionalEvaluator Untrue(bool condition, Func<int> truthyFunc)
        {
            ConditionalEvaluator not =
                new ConditionalEvaluator(condition, truthyFunc, null, Condition.Not);
            not.Also = this;
            ce.Add(not);
            return not;
        }

        public ConditionalEvaluator Untrue(Func<int> truthyFunc)
        {
            ConditionalEvaluator not =
                new ConditionalEvaluator(true, truthyFunc, null, Condition.Not);
            not.Also = this;
            ce.Add(not);
            return not;
        }

        public void Evaluate(bool preConditions = true)
        {
            bool FuncsRun = false;
            switch (this.conditionType)
            {
                case Condition.Or:
                    if (preConditions)
                    {
                        try { truthyFunc(); } catch (Exception) { }
                        FuncsRun = true;
                    }
                    break;
                case Condition.And:
                    if (preConditions == false)
                    {
                        try { falsyFunc(); } catch (Exception) { }
                        FuncsRun = true;
                    }
                    break;
                case Condition.Not:
                    if (preConditions == true)
                    {
                        try { falsyFunc(); } catch (Exception) { }
                        FuncsRun = true;
                    }
                    break;
            }

            if (!FuncsRun)
            {
                RunFuncs();
            }

            switch (this.conditionType)
            {
                case Condition.Or:
                    preConditions = preConditions | this.condition;
                    break;
                case Condition.And:
                    preConditions = preConditions & this.condition;
                    break;
                case Condition.Not:
                    preConditions = !preConditions;
                    break;
            }

            EvaluateSubConditions(preConditions);
        }

        private void RunFuncs()
        {
            if (this.condition)
            {
                try { truthyFunc(); } catch (Exception) { }
            }
            else
            {
                try { falsyFunc(); } catch (Exception) { }
            }
        }

        private void EvaluateSubConditions(bool preCondition)
        {
            foreach (var condition in ce)
            {
                condition.Evaluate(preCondition);
            }
        }
    }
}
