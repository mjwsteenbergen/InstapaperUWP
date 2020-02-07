using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstapaperTest
{
    [TestClass]
    public class E2Etest : BaseTest
    {
        [UITestMethod]
        public void TestMinimalism()
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
}", @"<section>
        <div>
            <div>
                <div>
                    <h3>Disqus</h3>
                    <figure><img src=""https://volument.com/blog/img/pie.svg""></figure>
                    <h4>446K</h4>
                    <p>Custom: 3.2K</p>
                </div>
                <div>
                    <h3>Optimizely</h3>
                    <figure><img src=""https://volument.com/blog/img/pie.svg""></figure>
                    <h4>798K</h4>
                    <p>Volument: 3.9K</p>
                </div>
                <div>
                    <h3>Sentry</h3>
                    <figure><img src=""https://volument.com/blog/img/pie.svg""></figure>
                    <h4>58K</h4>
                    <p>Custom: 0.2K</p>
                </div>
            </div>
        </div>
    </section>");
        }

        [UITestMethod]
        public void TestMinimalism2()
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
}", @"<div>
                <p>According to <a
                        href=""https://httparchive.org/reports/page-weight?start=earliest&amp;end=latest&amp;view=list"">HTTP
                        archive</a> the average size of a web page has gone up from <b>460K</b> to <b>1850K</b> in nine
                    years. The internet is four times fatter.</p>
                <figure><img src=""https://volument.com/blog/img/page-weight.png"">
                    <figcaption>Timeseries of total kilobytes. Sudden drops are mostly changes in measurement strategy.
                    </figcaption>
                </figure>

                <p>If you are struggling to find a market for your product, do what nobody else seems to do: choose
                    minimalism as your leading product development strategy. Start making less, but significantly better
                    stuff.</p>
                <p>Listen to <b>Steve Jobs</b> or <b>Dieter Rams</b> on product design, <b>Salvatore Sanfilippo</b> on
                    programming, and <b>Seth Godin</b> on content.</p>
                <p>Minimalism is a sparse human skill to strip down everything to bare essentials. It's the ability to
                    say no to 99% of things. It's hard, but it makes a difference. Likely more than you think.</p>
                <blockquote><q>Less is More</q>
                    <div>
                        <p>Mies van der Rohe</p>
                        <p>Pioneers of modernist architecture</p>
                    </div>
                </blockquote>


            </div>");
        }

        [UITestMethod]
        public void TestNews()
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
}", @"<p>
	<strong>Every minute of every day,</strong> everywhere on the planet, dozens of companies — largely unregulated, little scrutinized — are logging the movements of tens of millions of people with mobile phones and storing the information in gigantic data files. The Times <a href=""https://www.nytimes.com/privacy-project"">Privacy Project</a> obtained one such file, by far the largest and most sensitive ever to be reviewed by journalists. It holds more than 50 billion location pings from the phones of more than 12 million Americans as they moved through several major cities, including Washington, New York, San Francisco and Los Angeles.
</p>
<p>
	Each piece of information in this file represents the precise location of a single smartphone over a period of several months in 2016 and 2017. The data was provided to Times Opinion by sources who asked to remain anonymous because they were not authorized to share it and could face severe penalties for doing so. The sources of the information said they had grown alarmed about how it might be abused and urgently wanted to inform the public and lawmakers.
</p>
<p>
	<em><em><em>[Related: </em><a href=""https://www.nytimes.com/interactive/2019/12/20/opinion/location-data-national-security.html"">How to Track President Trump</a><em> —</em><em> Read more about the national security risks found in the data.]</em></em>
</em></p>
<p>
	
</p>
<p>
	After spending months sifting through the data, tracking the movements of people across the country and speaking with dozens of data companies, technologists, lawyers and academics who study this field, we feel the same sense of alarm. In the cities that the data file covers, it tracks people from nearly every neighborhood and block, whether they live in mobile homes in Alexandria, Va., or luxury towers in Manhattan.
</p>
<p>
	One search turned up more than a dozen people visiting the Playboy Mansion, some overnight. Without much effort we spotted visitors to the estates of Johnny Depp, Tiger Woods and Arnold Schwarzenegger, connecting the devices’ owners to the residences indefinitely.
</p>");
        }
    }
}
