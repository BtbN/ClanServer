using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ClanServer.Routing
{
    public class XrpcCallAttribute : Attribute
    {
        public string XrpcMethod { get; set; }

        public XrpcCallAttribute(string method)
        {
            XrpcMethod = method;
        }
    }

    public class XrpcCallActionConstraint : IActionConstraint, IActionConstraintMetadata
    {
        private readonly string method;

        public XrpcCallActionConstraint(string method)
        {
            this.method = method;
        }

        public int Order => 0;

        public bool Accept(ActionConstraintContext context)
        {
            var query = context.RouteContext.HttpContext.Request.Query;

            return query.TryGetValue("f", out var val) && val == method;
        }
    }

    public class XrpcCallConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            var xrpcCall = action.Attributes.OfType<XrpcCallAttribute>().FirstOrDefault();
            if (xrpcCall == null)
                return;

            foreach (var selector in action.Selectors)
            {
                selector.ActionConstraints.Add(new XrpcCallActionConstraint(xrpcCall.XrpcMethod));
            }
        }
    }
}
