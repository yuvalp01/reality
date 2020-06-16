using Nadlan.Models;
using Nadlan.Models.Issues;
using Nadlan.Models.Security;
using System;
using System.Collections.Generic;

namespace Nadlan.MockData
{
    public class MockIssues
    {
        public static List<Issue> GetAllIssues()
        {
            List<Issue> issues = new List<Issue>
            {
                new Issue
                {
                    Id =1,
                    Title = "Some old issue (closed)",
                    Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Priority =2 ,
                    Apartment = new Apartment{ Id=1, Address="Mavromichali" },
                    DateOpen =new DateTime(1,1,2020),
                    DateClose = new DateTime(1,2,2020)

                },
                new Issue
                {
                    Id =2,
                    Title = "Some urgent issue",
                    Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Priority =1 ,
                    Apartment = new Apartment{ Id=1, Address="Mavromichali" },
                    DateOpen =new DateTime(1,3,2020),

                },
                 new Issue
                {
                    Id =3,
                    Title = "Not urgent at all",
                    Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Priority =3 ,
                    Apartment = new Apartment{ Id=3, Address="Ippokratus" },
                    DateOpen =new DateTime(12,5,2020),
                 
                },
               new Issue
                {
                    Id =4,
                    Title = "Not so urgent, the title is longer. We need to make sure the lines won't break.",
                    Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Priority =2 ,
                    Apartment = new Apartment{ Id=3, Address="Bouboulinas" },
                    DateOpen =new DateTime(1,1,2020),

                }
             };
            return issues;


        }

        public static List<IssueItem> GetIssueItemsByIssueId(int issueId)
        {
            List<IssueItem> issueItems = new List<IssueItem>
            {
                new IssueItem{ Id=1, Description="I solved the issue", IssueId=1, User = new AppUser{UserId=Guid.NewGuid(), UserName="stella" } },
                new IssueItem{ Id=2, IssueId=1, Description="Ok great thanks", User = new AppUser{UserId=Guid.NewGuid(), UserName="yuval" } },
                
                new IssueItem{ Id=3, IssueId=2, Description="Should I do that?", User = new AppUser{UserId=Guid.NewGuid(), UserName="stella" } },
                new IssueItem{ Id=4, IssueId=2,  Description="Yes, you can do that, but don't do that and that and that, thanks.", User = new AppUser{UserId=Guid.NewGuid(), UserName="yuval" } },
                new IssueItem{ Id=5, IssueId=2, Description="Ok no problem, I'll do that!", User = new AppUser{UserId=Guid.NewGuid(), UserName="stella" } },
                new IssueItem{ Id=6, IssueId=2, Description="Great thanks :)", User = new AppUser{UserId=Guid.NewGuid(), UserName="yuval" } },

                new IssueItem{ Id=7, IssueId=3, Description="I talked to him and he said that blablbllaalala", User = new AppUser{UserId=Guid.NewGuid(), UserName="stella" } },
                new IssueItem{ Id=8, IssueId=3, Description="Thanks for the update", User = new AppUser{UserId=Guid.NewGuid(), UserName="yuval" } },

                new IssueItem{ Id=9, IssueId=4, Description="Any progress?", User = new AppUser{UserId=Guid.NewGuid(), UserName="yuval" } },

            };
            return issueItems;
        }
    }
}
