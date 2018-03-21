using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Fluid;
using Fluid.Ast;
using Fluid.Values;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Cache;
using OrchardCore.Liquid.Ast;

namespace OrchardCore.DynamicCache.Liquid
{
    public class CacheExpiresSlidingTag : ExpressionArgumentsTag
    {
        public override async Task<Completion> WriteToAsync(TextWriter writer, TextEncoder encoder, TemplateContext context, Expression expression, FilterArgument[] args)
        {
            if (!context.AmbientValues.TryGetValue("Services", out var servicesObj))
            {
                throw new ArgumentException("Services missing while invoking 'cache_expires_sliding' tag");
            }

            var services = servicesObj as IServiceProvider;

            var cacheScopeManager = services.GetService<ICacheScopeManager>();

            if (cacheScopeManager == null)
            {
                return Completion.Normal;
            }

            TimeSpan value;
            var input = await expression.EvaluateAsync(context);

            if (input.Type == FluidValues.String)
            {
                var stringValue = input.ToStringValue();
                if (TimeSpan.TryParse(stringValue, out value))
                {
                    return Completion.Normal;
                }
            }
            else
            {
                var objectValue = input.ToObjectValue() as TimeSpan?;

                if (!objectValue.HasValue)
                {
                    return Completion.Normal;
                }

                value = objectValue.Value;
            }

            cacheScopeManager.WithExpirySliding(value);

            return Completion.Normal;
        }
    }
}