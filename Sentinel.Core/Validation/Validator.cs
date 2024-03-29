﻿using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sentinel.Core.Validation
{
    public class Validator
    {
        public readonly IServiceProvider services;

        public Validator(IServiceProvider services)
        {
            this.services = services;
        }

        public void Validate<T>(T itemToValidate)
        {
            BaseValidator<T> validator = services.GetRequiredService<BaseValidator<T>>();
            validator.Validate(itemToValidate);
        }

        public void IsValid<T>(T itemToValidate)
        {
            BaseValidator<T> validator = services.GetRequiredService<BaseValidator<T>>();
            validator.IsValid(itemToValidate);
        }
    }
}