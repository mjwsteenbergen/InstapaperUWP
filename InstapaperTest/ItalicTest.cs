
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace InstapaperTest
{
    [TestClass]
    public class ItalicTest: BaseTest
    {
        [UITestMethod]
        public void Italic()
        {
            Compare(@"
p {
    Italic {
        Run {
            'hello'
        }
    }
}", "<i>hello</i>");
        }

        [UITestMethod]
        public void ItalicInSpan()
        {
            Compare(@"
p {
    Italic {
        Run {
            'hello '
        }
    },
    Span {
        Run {
            'this is a test'
        }
    }
}", "<span><i>hello</i> this is a test</span>");
        }

        [UITestMethod]
        public void ItalicInSpanNoSpace()
        {
            Compare(@"
p {
    Italic {
        Run {
            'hello '
        }
    },
    Span {
        Run {
            'this is a test'
        }
    }
}", "<span><i>hello</i>this is a test</span>");
        }

        [UITestMethod]
        public void ItalicInSpanEnd()
        {
            Compare(@"
p {
    Span {
        Run {
            'this is a test'
        }
    },
    Italic {
        Run {
            ' hello'
        }
    }
}", "<span>this is a test<i>hello</i></span>");
        }
    }
}
