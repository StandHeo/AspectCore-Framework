﻿using System.Linq;
using System.Reflection;
using AspectCore.Abstractions.Internal;

namespace AspectCore.Abstractions.Internal
{
    public class AttributeAspectValidationHandler : IAspectValidationHandler
    {
        public int Order { get; } = 13;

        public bool Invoke(MethodInfo method, AspectValidationDelegate next)
        {
            if (method.IsPropertyBinding())
            {
                return false;
            }

            var declaringType = method.DeclaringType.GetTypeInfo();

            if (IsAttributeAspect(declaringType) || IsAttributeAspect(method))
            {
                return true;
            }

            return next(method);
        }

        private bool IsAttributeAspect(MemberInfo member)
        {
            return member.CustomAttributes.Any(data => typeof(IInterceptor).GetTypeInfo().IsAssignableFrom(data.AttributeType.GetTypeInfo()));
        }
    }
}