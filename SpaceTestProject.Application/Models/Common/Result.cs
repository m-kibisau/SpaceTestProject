﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTestProject.Application.Models.Common
{
    public class Result<T>
    {
        public IReadOnlyCollection<BusinessMessage> Messages { get; set; }
        public T Data { get; set; }
        public bool IsSuccess() => Messages == null || !Messages.Any();
        public string GetMessageSummary() => string.Join(", ", Messages.Select(x => x.Message));

        private Result(T data) => Data = data;
        private Result(IEnumerable<BusinessMessage> messages) => Messages = messages.ToList();


        public static Result<T> Success(T data) => new Result<T>(data);

        public static Result<T> Fail(IEnumerable<BusinessMessage> messages) => new Result<T>(messages);

        public static Result<T> Fail(string message) => new Result<T>(new List<BusinessMessage>()
        {
            new BusinessMessage() { Message = message }
        });

        public static Result<T> Forbidden(IEnumerable<BusinessMessage> messages)
        {
            messages.ToList().ForEach(x => x.HttpCode = 403);
            return new Result<T>(messages);
        }

        public static Result<T> Forbidden(string message) => new Result<T>(new List<BusinessMessage>()
        {
            new BusinessMessage()
            {
                HttpCode = new int?(403),
                Message = message
            }
        });

    }
}
