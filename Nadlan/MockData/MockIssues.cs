using Microsoft.EntityFrameworkCore;
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
            List<Issue> isueList = new List<Issue>
            {
                new Issue
                {
                    Id =1,
                    Title = "Some old issue (closed)",
                    Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Priority =2 ,
                    ApartmentId = 1,
                    DateOpen =new DateTime(2020,1,1),
                    DateClose = new DateTime(2020,2,1)

                },
                new Issue
                {
                    Id =2,
                    Title = "Some urgent issue",
                    Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Priority =1 ,
                    ApartmentId = 1,
                    DateOpen =new DateTime(2020,3,1),

                },
                 new Issue
                {
                    Id =3,
                    Title = "Not urgent at all",
                    Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Priority =3 ,
                    ApartmentId = 3,
                    DateOpen =new DateTime(2020,5,12),

                },
               new Issue
                {
                    Id =4,
                    Title = "Not so urgent, the title is longer. We need to make sure the lines won't break.",
                    Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Priority =2 ,
                    ApartmentId = 5,
                    DateOpen =new DateTime(2020,1,1),

                },
              new Issue
                {
                    Id =5,
                    Title = "Not so urgent, the title is longer. We need to make sure the lines won't break.",
                    Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Priority =2 ,
                    ApartmentId = 5,
                    DateOpen =new DateTime(2020,1,1),
                    IsDeleted = true

                }
             };
            return isueList;


        }

        public static List<Message> GetAllMessages()
        {
            List<Message> messages = new List<Message>
            {
                new Message{ Id=1, Description="I solved the issue", IssueId=1, User = new AppUser{UserId=Guid.NewGuid(), UserName="stella" } },
                new Message{ Id=2, IssueId=1, Description="Ok great thanks", User = new AppUser{UserId=Guid.NewGuid(), UserName="yuval" } },

                new Message{ Id=3, IssueId=2, Description="Should I do that?", User = new AppUser{UserId=Guid.NewGuid(), UserName="stella" } },
                new Message{ Id=4, IssueId=2,  Description="Yes, you can do that, but don't do that and that and that, thanks.", User = new AppUser{UserId=Guid.NewGuid(), UserName="yuval" } },
                new Message{ Id=5, IssueId=2, Description="Ok no problem, I'll do that!", User = new AppUser{UserId=Guid.NewGuid(), UserName="stella" } },
                new Message{ Id=6, IssueId=2, Description="Great thanks :)", User = new AppUser{UserId=Guid.NewGuid(), UserName="yuval" } },

                new Message{ Id=7, IssueId=3, Description="I talked to him and he said that blablbllaalala", User = new AppUser{UserId=Guid.NewGuid(), UserName="stella" } },
                new Message{ Id=8, IssueId=3, Description="Thanks for the update", User = new AppUser{UserId=Guid.NewGuid(), UserName="yuval" } },

                new Message{ Id=9, IssueId=4, Description="Any progress?", User = new AppUser{UserId=Guid.NewGuid(), UserName="yuval" } },

            };
            return messages;
        }
    }
}
