﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace eclectica.co.uk.Test
{
    public static class Extensions {

        public static void ShouldEqual<T>(this T actualValue, T expectedValue)
        {
            Assert.AreEqual(expectedValue, actualValue);
        }
        
    }
}