using Nadlan.Models.Issues;
using Nadlan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.BusinessLogic
{
    public class IssueFilter
    {
        public Func<Issue, bool> GetAllIssues()
        {
            Func<Issue, bool> basicPredicate = t =>
                        !t.IsDeleted;
            return basicPredicate;
        }

        public Func<Issue, bool> GetOpenIssues()
        {
            Func<Issue, bool> basicPredicate = t =>
                        t.IsDeleted == false
                     && t.DateClose == null;
            return basicPredicate;
        }

        //public Func<Message, bool> GetItemsOfOpenIssues()
        //{
        //    Func<Message, bool> basicPredicate = t =>
        //                t.IsDeleted == false
        //             && t.Issue.IsDeleted == false
        //             && t.Issue.DateClose == null;
        //    return basicPredicate;
        //}


        public Func<Message, bool> GetAllMessages()
        {
            Func<Message, bool> basicPredicate = t =>
                        !t.IsDeleted;
            return basicPredicate;
        }

        public Func<Message, bool> GetItemsOfIssues()
        {
            Func<Message, bool> basicPredicate = t =>
                        t.IsDeleted == false;
                     //&& t.Issue.IsDeleted == false;
            return basicPredicate;
        }


    }
}
