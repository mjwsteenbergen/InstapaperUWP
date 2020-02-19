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

        [UITestMethod]
        public void TestBillGates()
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
                            
                            
                            
    






<div>
    <div>
    </div>
</div>


<div>
</div>
<div>



</div>
















<img src=""https://conversion.adshop.infolinks.com/conversion/?pixel=1&amp;aid=461419"">

<div>
    <div>
        
    </div>
    
    <div>
        TGN_CH_BO_BGChapterMenuHolder
    </div>
</div>


<div>
    <div>
        <div>
            <span>|</span>
        </div>
        <div>
            <div>1</div>
            
        </div>
        <div>
            <div>
                <div>
                    
                    
                    
                </div>
            </div>
        </div>
    </div>
</div>
<div>
    <div><span>Languages</span><div><img src=""https://www.gatesnotes.com/img/language01.svg""><img src=""https://www.gatesnotes.com/img/language01w.svg""></div></div>
    <div>
        <div>PDF Download</div>
    </div>
</div>

<div>
    <div>
        <div>

                    

                    

                    

        </div>
        <div>
            
            
            
        </div>
    </div>
    <span>Reflecting on the first two decades of our foundation</span>

<p>When we started our foundation 20 years ago, the world was, in many ways, very different from the one we live in now. It was before 9/11, before the Great Recession, and before the rise of social media.</p>

<p>Then, as now, there was no shortage of worthy causes, and there was a good argument to be made for investing in many of them. We’d known for a while that we wanted to give away the majority of our wealth from Microsoft and use it to make people’s lives better. The challenge, of course, was how to do that in a meaningful and high-impact way.</p>

<p>As we were thinking about what our philanthropic priorities would be, we spent a lot of time meeting with experts and poring over reports.  What we learned convinced us that the world should be doing more to address the needs of its poorest people. At the core of our foundation’s work is the idea that every person deserves the chance to live a healthy and productive life. Twenty years later, despite how much things have changed, that is still our most important driving principle.</p>





<div>
    <div>“At the core of our foundation’s work is the idea that every person deserves the chance to live a healthy and productive life.”</div>
    <div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Fb.svg""></div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Tw.svg""></div>
    </div>
    
    
    
</div>


<span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/m1_v3"">
</span>

<p>There is no question that this new decade is beginning at a time of tremendous unrest and uncertainty around the world. But even in a moment as challenging as this one—in fact, especially in a moment like this one—we remain committed to supporting advocates, researchers, government officials, and frontline workers who are making a healthy and productive life possible for more people in more places.</p>

<p>For the last 20 years, our foundation has focused on improving health around the world and strengthening the public education system in the United States because we believe that health and education are key to a healthier, better, and more equal world. Disease is both a symptom and a cause of inequality, while public education is a driver of equality.</p>

<p>We know that philanthropy can never—and should never—take the place of governments or the private sector. We do believe it has a unique role to play in driving progress, though.</p>

<p>At its best, philanthropy takes risks that governments can’t and corporations won’t. Governments need to focus most of their resources on scaling proven solutions.</p>

<p>Businesses have fiduciary responsibilities to their shareholders. But foundations like ours have the freedom to test out ideas that might not otherwise get tried, some of which may lead to breakthroughs.</p>

<span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/b1_v3"">
</span>

<p>As always, Warren Buffett—a dear friend and longtime source of great advice—put it a little more colorfully. When he donated the bulk of his fortune to our foundation and joined us as a partner in its work, he urged us to “<span>swing for the fences</span>.”</p> 

<p>That’s a phrase many Americans will recognize from baseball. When you swing for the fences, you’re putting every ounce of strength into hitting the ball as far as possible. You know that your bat might miss the ball entirely—but that if you succeed in making contact, the rewards can be huge.</p>

<p>That’s how we think about our philanthropy, too. The goal isn’t just incremental progress. It’s to put the full force of our efforts and resources behind the big bets that, if successful, will save and improve lives. </p>

<p>To be clear, the risks we take are different from the ones the true heroes of global progress take all the time: the health workers who brave war zones to get vaccines to children who need them, the teachers who sign up to work in the most challenging schools, the women in the world’s poorest places who stand up against cultural norms and traditions designed to keep them down. What they do requires personal sacrifices we never have to make—and we try to honor them by supporting innovations that might one day make their lives easier. </p>

<p>Altogether, our foundation has spent $53.8 billion over the last 20 years. On the whole, we’re thrilled with what it’s accomplished. But has every dollar we’ve spent had the effect we’ve hoped for? No. We’ve had our share of disappointments, setbacks, and surprises. We think it’s important to be transparent about our failures as well as our successes—and it’s important to share what we’ve learned.</p>

<div>




<div>
    <div>
        <img src="""">
                

                

                

        
    </div>
    <div>
        
        <div>
            
            
            
        </div>
    </div>
    <div>
    </div>
</div>

</div>

<p>In this year’s letter, we write about the work we’ve done on health and education and why we think the risks we’ve taken have set us up for future progress. We also write about two issues that have emerged as priorities for us—the climate crisis and gender equality—and how they will factor into our next 20 years.
</p>

<p>Some of the very first investments we made as philanthropists were aimed at correcting inequities in global health. So, we’ll begin this letter there, too.</p>
</div>

<div>
    <div>
        <div>

                    

                    

                    

        </div>
        <div>
            
            <div>Global health</div>
            
        </div>
    </div>
    

<div><span>
Melinda:
</span><span>
When we first started working in global health, we were shocked to learn how many children in low-income countries were still dying from diseases that could have been prevented with vaccines that were widely available in countries like the U.S. It drove home for us that the challenges of poverty and disease are always connected.
</span></div>

<p>Since this wasn’t something that markets and governments were solving on their own, we saw an opportunity for philanthropic dollars to help. 
We worked with the World Health Organization, the World Bank, and UNICEF to create <a href=""http://www.gavi.org/"">Gavi, the Vaccine Alliance</a>. Gavi brings together governments and other organizations to raise funds to buy vaccines and support low-income countries as they deliver them to children.</p>

<span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/b2_v3"">
</span>

<div><span>
Bill:
</span><span>
After World War II, the world came together to create a series of international organizations aimed at increasing economic and military cooperation among nations, including the UN, WHO, and NATO. Gavi was a chance to drive similar cooperation around getting vaccines to kids.
</span></div>

<p>We weren’t entirely sure what to expect. Thanks to Microsoft, I was familiar with the risks of starting a new organization. The risks we were taking with Gavi were different, though. Instead of trying to introduce a new product and appeal to customers, we were trying to prove to the world that an international partnership for vaccines was not only possible but necessary. If we failed, we could discourage governments and other funders from investing in future efforts.</p>

<p>There were so many questions. Could we really raise enough money to convince manufacturers to supply vaccines that developing countries could afford? And even if we did, could we get countries to take on the difficult task of getting new and underused vaccines out there to children?</p>

<span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/m3_v3"">
</span>

<p>The answers to both questions turned out to be a resounding <i>yes</i>. By 2019, Gavi had helped vaccinate more than 760 million children and prevent 13 million deaths. It has also succeeded in bringing more vaccines and supplies into the market while lowering prices. For example, a single dose of the pentavalent vaccine, which protects against five deadly infections, used to cost $3.65. It now costs less than a dollar.</p>




<div>
    <div>
        <div class=""video_frame""><iframe src=""https://www.youtube.com/embed/hh6xTguRn9o?rel=0&amp;modestbranding=1&amp;playsinline=1&amp;color=white&amp;showinfo=0""></iframe></div>
    </div>
</div>


<div><span>
Melinda:
</span><span>
Today, 86 percent of children around the world receive basic immunizations. That’s more than ever before. 
But reaching the last 14 percent is going to be much harder than reaching the first 86 percent. The children in this group are some of the most marginalized children in the world.
</span></div>

<p>Some of them live in fragile states where conflict prevents the health system from working well for anyone. Others live in remote rural areas. Frustratingly, some live just a few hundred meters from a health facility but are invisible to the health system. (Picture, for example, the child of recent migrants living in overcrowded, impoverished areas of Nairobi or Rio de Janeiro.) Gavi is now increasingly focused on working with countries to take a more targeted approach to the districts where unvaccinated children are concentrated.</p>





<div>
    <div>“We think going big on Gavi was one of the best decisions we’ve ever made—and we’re thrilled with the return we’ve seen on our investment.”</div>
    <div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Fb.svg""></div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Tw.svg""></div>
    </div>
    
    
    
</div>


<p>As Gavi raises funds for its next five years of work, we want to encourage more donors to commit to extending this incredible success story to <i>all</i> children. More funding will allow Gavi to save more lives. We think going big on Gavi was one of the best decisions we’ve ever made—and we’re thrilled with the return we’ve seen on our investment.</p>

<div><span>
Bill:
</span><span>
Our work on vaccines has parallels with another area we’ve been heavily involved with since the beginning: HIV and AIDS.
</span></div>

<p>When our foundation opened its doors, the AIDS death rate in the rich world had finally started going down, thanks to new treatments. But as with vaccines, the tools that were saving lives in high-income countries weren’t available in low-income countries. The number of new infections in sub-Saharan Africa was skyrocketing. I remember reading a horrifying <i>Newsweek</i> article about how the virus would turn an entire generation of children into orphans.</p>





<div>
    <div>
        <img src="""">
                

                

                

        
    </div>
    <div>
        
        <div>
            
            
            
        </div>
    </div>
    <div>
    </div>
</div>

<span><i>Newsweek</i> cover, January 17, 2000</span>
<span><i>Newsweek</i> cover, January 17, 2000</span>

<p>In response to the growing epidemic—as well as the need to address two other big killers—in 2002 we helped support the creation of a new organization called the <a href=""http://www.theglobalfund.org/"">Global Fund to Fight AIDS, Tuberculosis, and Malaria</a>. It had a similar goal to Gavi’s: to deliver medicines, technologies, and programs that save lives in low-income countries. It was also risky for all the same reasons.</p>

<span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/m4_v4"">
</span>

<p>But just like Gavi, the Global Fund has proven to be a tremendous success. In 2018 alone, nearly 19 million people received lifesaving HIV treatment in countries where the organization invests.</p>

<span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/b3_v3"">
</span>


<p>Once the Global Fund was established, we knew the world had a pipeline to get new innovations out to the places that needed them the most. So, along with supporting the Global Fund, our foundation invested in the development of new tools.</p>

<p>In the beginning, we put a lot of resources into HIV preventatives that needed to be taken every day. For a lot of reasons, those didn’t turn out as we hoped.</p>

<span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/m5_v3"">
</span>

<p>For example, we were optimistic that vaginal gels could help prevent infection, but they weren’t effective at stopping transmission of the disease. And while there is now a daily preventative pill that is 99 percent effective at protecting against the disease if taken consistently, it hasn’t made a real dent in the epidemic in low- and middle-income countries. Local health programs have struggled to deliver a daily pill in a way that’s appealing and fits into people’s lives.</p>

<p>Today we’re focused on longer-lasting preventatives. Imagine if, instead of having to take a pill every day, a person could get one injection every other month, an implant in his or her arm, or even a vaccine to entirely remove the risk of getting the virus.</p>

<p>Our foundation is also focused on longer-lasting treatment options. Thanks to major advances, an HIV-positive person receiving treatment now has the same expected lifespan as someone without HIV. But, just like with today’s preventatives, the medication has to be taken every day. We’re looking for new treatments that can be taken less frequently, as much as a year apart.</p>

<p>Even if we perfect long-lasting options, there are still a number of challenges to tackle before we truly reverse the course of the epidemic.</p>





<div>
    <div>
        <img src="""">
                

                

                

        
    </div>
    <div>
        
        <div>
            
            
            
        </div>
    </div>
    <div>
    </div>
</div>

<span>During a visit to Gugulethu Community Health Clinic last year, the staff told us about the HIV and tuberculosis patients they see in Cape Town, South Africa.</span>

 
<div><span>
Melinda:
</span><span>
In 2003, we visited an HIV clinic in Botswana that, at the time, was one of the biggest HIV clinics on the continent. We’ll always remember that trip as a sobering lesson about the social and structural drivers of the disease.
</span></div>

<p>We spent time with a Dutch doctor who told us about a local Botswanan woman he and his wife had employed in their home. One day, the woman told them she was going to her village for a visit—and she never came back. When the concerned couple went looking for her, they were shocked to learn she had died of AIDS.</p>

<p>It wasn’t the fact that she <i>had</i> AIDS that shocked them. It was the fact that she died without seeking treatment, even though she had a personal connection to the clinic and would have had access to the best care available. But that’s how devastating the stigma surrounding AIDS was. It could literally be deadly.</p>

<p>That story has stayed with us. And by complicating our understanding of the epidemic, it clarified our call to action.</p>

<p>The reality is that in the fight against HIV, biomedical interventions alone will never be enough.   Our response also needs to reflect what matters to people, what’s keeping them from seeking prevention and treatment services, and why the tools that prove effective in clinical trials don’t always make a difference in the context of their everyday lives.</p>

<p>We know, for example, that across southern and eastern Africa, adolescent girls and young women account for a disproportionate number of new HIV infections. Poverty, violence, and gender norms all play a role in why.</p>

<div>




<div>
    <div>
        <img src="""">
                

                

                

        
    </div>
    <div>
        
        <div>
            
            
            
        </div>
    </div>
    <div>
    </div>
</div>

</div>


<p>But for all that we do know about these girls, there’s a lot we don’t. We know how their lives look through our eyes. We don’t have a lot of data about what the world looks like through theirs. And that hampers our ability to develop effective solutions for them—biomedical and otherwise.</p>

<p>Fortunately, the research is starting to catch up to this reality. When I was in Johannesburg last October, I spent time with a foundation partner that is working to close this data gap and engage adolescent girls and young women to co-design treatment and prevention services that will better meet their needs.</p>

<p>Our foundation has also partnered with a U.S. government-backed program called <a href=""https://www.usaid.gov/global-health/health-areas/hiv-and-aids/technical-areas/dreams"">DREAMS</a>, an acronym for Determined Resilient Empowered AIDS-Free Mentored and Safe. As the name suggests, the program takes a broad approach to HIV prevention. It also addresses, for example, financial literacy, entrepreneurship, and ending gender-based violence—all of which can help women and girls live healthy, thriving, and HIV-free lives.</p>

<p>Over the last 20 years, science has made incredible advances against HIV. Crucially, the world’s understanding of how to deploy that science is moving forward, too.</p>

<div><span>
Bill:
</span><span>
Global health will always be a core focus of our foundation. This work will only become more important in the future, as climate change makes more people susceptible to disease. (I’ll have a bit more to say about this later in the letter.)
</span></div>

<p>Along with our investments in vaccines and HIV, we will continue to support progress on other diseases, like malaria, tuberculosis, and polio (through our partnership with the <a href=""http://polioeradication.org/"">Global Polio Eradication Initiative</a>). We’ll fund new advances in family planning and maternal and newborn health, and we’ll explore new ways of preventing the scourge of malnutrition.</p>





<div>
    <div>“As people become healthier, their lives improve in other ways. And the world becomes better and more equal as a result.”</div>
    <div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Fb.svg""></div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Tw.svg""></div>
    </div>
    
    
    
</div>


<p>That’s because improvements in health are key to lifting people out of poverty. As people become healthier, their lives improve in other ways. And the world becomes better and more equal as a result.</p>
</div>

<div>
    <div>
        <div>

                    

                    

                    

        </div>
        <div>
            
            <div>Education</div>
            
        </div>
    </div>
    <span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/b3_2_v3"">
</span>

<div><span>
Melinda:
</span><span>
Bill and I always knew that our foundation’s U.S. work would focus mostly on K-12 and postsecondary education. Success in America is a complex equation with too many variables to count—race, gender, your ZIP code, your parents’ income levels—but education is an incredibly important part of that equation.
</span></div>

<p>Both of us had the chance to attend excellent schools, and we know how many doors that opened for us. We also know that millions of Americans, especially low-income students and students of color, don’t have that same opportunity.</p>

<p>Experts, of course, have a much more rigorous vocabulary to describe this situation. In 2001, I met an educator named Deborah Meier who had a big impact on me. Her book <a href=""https://www.goodreads.com/book/show/219249.The_Power_of_Their_Ideas""><i>The Power of Their Ideas</i></a> helped me understand why public schools are not only an important equalizer but the engine of a thriving democracy. A democracy requires equal participation from everyone, she writes. That means when our public schools fail to prepare students to fully participate in public life, they fail our country, too.</p>

<p>I think about that a lot. It really helps drive home the stakes of this work for me.</p>

<p>If you’d asked us 20 years ago, we would have guessed that global health would be our foundation’s riskiest work, and our U.S. education work would be our surest bet. In fact, it has turned out just the opposite.</p>

<p>In global health, there’s a lot of evidence that the world is on the right path—like the dramatic decline in childhood deaths, for example. When it comes to U.S. education, though, we’re not yet seeing the kind of bottom-line impact we expected. The status quo is still failing American students.</p>

<p>Consider this: The average American primary school classroom has 21 students. Currently, 18 of those 21 complete high school with a diploma or an equivalent credential (which is a significant improvement since 2000), but only 13 start any kind of postsecondary program within a year of graduating. Only seven will earn a degree from a four-year-program within six years.</p>

<div>




<div>
    <div>
        <img src="""">
                

                

                

        
    </div>
    <div>
        
        <div>
            
            
            
        </div>
    </div>
    <div>
    </div>
</div>

</div>

 
<p>It gets worse when you disaggregate by race. If every student in our classroom is Latinx, only six will finish their four-year degree program within six years. For a classroom of Black students, the number is just four.</p>

<p>The fact that progress has been harder to achieve than we hoped is no reason to give up, though. Just the opposite. We believe the risk of not doing everything we can to help students reach their full potential is much, much greater.</p>





<div>
    <div>“The fact that progress has been harder to achieve than we hoped is no reason to give up, though. Just the opposite.”</div>
    <div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Fb.svg""></div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Tw.svg""></div>
    </div>
    
    
    
</div>


<p>We certainly understand why many people are skeptical about the idea of billionaire philanthropists designing classroom innovations or setting education policy. Frankly, we are, too. Bill and I have always been clear that our role isn’t to generate ideas ourselves; it’s to support innovation driven by people who have spent their careers working in education: teachers, administrators, researchers, and community leaders.</p>

<p>But one thing that makes improving education tricky is that even among people who work on the issue, there isn’t much agreement on what works and what doesn’t.</p>

<p>In global health, we know that if children receive the measles vaccine, they will be protected against the disease, which means they’re more likely to survive. But there’s no consensus on cause and effect in education. Are charter schools good or bad? Should the school day be shorter or longer? Is this lesson plan for fractions better than that one? Educators haven’t been able to answer those questions with enough certainty to establish clear best practices.</p>

<p>It’s also hard to isolate any single intervention and say it made all the difference. Getting a child through high school requires at least 13 years of instruction enabled by hundreds of teachers, administrators, and local, state, and national policymakers. The process is so cumulative that changing the ultimate outcome requires intervention at many different stages.</p>

<p>Even so, we <i>have</i> seen some signs of progress. Among other things, we’ve helped support some improvements in curriculum, gotten smarter about keeping kids from dropping out, and deepened our understanding of what makes a great teacher great and can make a good teacher better. (Bill goes into even more reasons to be optimistic below.)</p>

<span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/b4_v3"">
</span>

<p>We’re also proud of our <a href=""https://gmsp.org/"">Gates Millennium Scholars Program</a>, which provided full college scholarships to 20,000 students of color. We’ve had the chance to meet some of these scholars, and it’s always a very moving experience. One, Kaira Kelly, told me she “had never really dreamed of going to college” before becoming a Gates Millennium Scholar. When I met her, she was getting a master’s degree in education and brimming with plans about how to pay forward the investment made in her.</p>

<p>Although these scholarships made a huge difference in the lives of those 20,000 students, the reality is that tens of millions of other students passed through U.S. public schools during the 16 years we granted scholarships. That means we reached only a tiny percentage of them. Our goal is to help make a huge difference for <i>all</i> U.S. students, so we’ve pivoted most of our work from scholarships to areas that can have more impact for more students.</p>

<p>It’s an incredible thing to watch a young woman like Kaira—or the three Gates Millennium Scholars profiled below—tap into their potential. And it reinforces our commitment to supporting a public-school system that will ensure every student has the same opportunity they did.</p>




<div>
    <div>
        <div>
            <div><img src=""https://media.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/20190108-AL2020-GMS-Scholar01-001.ashx""></div>
            <div>KRISTINA ELLIS</div>
        </div>
        <div>
            <div><img src=""https://media.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/20190108-AL2020-GMS-Scholar02-001.ashx""></div>
            <div>DR. BRENDA CALDERON</div>
        </div>
        <div>
            <div><img src=""https://media.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/20190108-AL2020-GMS-Scholar03-001.ashx""></div>
            <div>DR. MARVIN CARR</div>
        </div>
    </div>
    <div>
        
    </div>
    <div>
        <div>
            <div>

<span>B.S. from Vanderbilt University; M.A. from Belmont University</span>

<span>“My mom, an immigrant from Venezuela, became a single mother after my dad passed from cancer. While my mom equipped me with an abundance of ambition and a strong work ethic, our family didn’t have the money to pay for college. The Gates family’s generosity has inspired me to pay it forward by building a business that has guided more than 100,000 students (and counting!) through the college application and scholarship process so that they too may graduate debt-free.”</span></div>
        </div>
        <div>
            <div><span>B.A. from UCLA.; M.A. from Loyola Marymount University; Ph.D. from George Washington University</span>

<span>“Being a Gates Millennium Scholar has truly been a life-changing experience for me. I went from being the first person in my family with a high school diploma to the first in my family with a doctorate degree, and the ripple effect it’s had on the rest of my family and community has been immeasurable. Today, I work at the U.S. Department of Education, where I oversee a $700 million grant program for states to improve language access services to students.”</span></div>
        </div>
        <div>
            <div><span>B.S. from Morgan State University; M.S. from University of Maryland, Baltimore County; Ph.D. from Morgan State University</span>

<span>“I never imagined getting a doctorate in engineering until the Gates Millennium Scholarship showed me that I could. I never imagined working at the White House—first as an unpaid intern and later as a staffer—had it not been for GMS covering my costs associated with room and board. While the GMS did not make me who I am, it unlocked an image in my head of who I could be and provided me the resources that enabled me to do the work to become that person. For this, I am forever grateful.”</span></div>
        </div>
    </div>
</div>



<div><span>
Bill:
</span><span>
So how exactly can we equip students with the tools they need to learn and thrive? We found out early on in our work that students need clear and consistent standards in order to master what they’re learning from year to year.
</span></div>

<p>We bet big on a set of standards called the Common Core. Nearly every state adopted them within two years of their release. But it quickly became clear that adoption alone wasn’t enough—something we should’ve anticipated. We thought that if states raised the standards the market would respond and develop new instructional materials that aligned with those standards. That didn’t happen, so we looked for ways to encourage the market.</p>

<p>After teachers told us they had no way of knowing whether a textbook met the new standards, our foundation backed a nonprofit organization called <a href=""https://www.edreports.org/"">EdReports</a>, which acts like a <i>Consumer Reports</i> for instructional materials. Now, any teacher can look up a textbook to see if it is high-quality and aligned with the standards. Schools have started purchasing more of the materials that best serve their students based on these reviews—and manufacturers, in turn, have begun creating more and better textbook options.</p>

<p>Beyond textbooks, we knew we needed to find other ways to better support teachers and students. For example, many teachers didn’t have access to the resources they needed to meet the new expectations. So, we looked for ways to provide more training and help them adjust their practice.</p>

<p>But if there’s one lesson we’ve learned about education after 20 years, it’s that scaling solutions is difficult. Much of our early work in education seemed to hit a ceiling. Once projects expanded to reach hundreds of thousands of students, we stopped seeing the results we hoped for.</p>

<p>It became clear to us that scaling in education doesn’t mean getting the same solution out to everyone. Our work needed to be tailored to the specific needs of teachers and students in the places we were trying to reach.</p>





<div>
    <div>“Our work needed to be tailored to the specific needs of teachers and students in the places we were trying to reach.”</div>
    <div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Fb.svg""></div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Tw.svg""></div>
    </div>
    
    
    
</div>


<p>We’ve shifted our primary focus in K-12 to locally driven solutions identified by networks of schools. Our hope is that these <a href=""https://k12education.gatesfoundation.org/what-we-do/networks-for-school-improvement/"">Networks for School Improvement</a> will increase the number of Black, Latinx, and low-income students who graduate from high school and pursue postsecondary opportunities.</p>

<p>So far, we’ve awarded $240 million across 30 networks. Many, but not all, are grouped by region. Each network includes eight to 20 schools and is focused on a goal of its choosing—for example, helping freshmen who aren’t “on track” to graduate get themselves on the right path.</p>

<p>The first year of high school is a critical moment. A freshman who fails no more than one course is four times more likely to graduate than one who fails two or more. Being “on track” in this way is more predictive of whether that student will graduate than race, wealth, or even test scores.</p>

<p>In 2018, I visited North-Grand High School in Chicago. The school serves students from neighborhoods that struggle with violence, hunger, and other challenges. It used to be ranked among the worst schools in the city.</p>





<div>
    <div>
        <img src="""">
                

                

                

        
    </div>
    <div>
        
        <div>
            
            
            
        </div>
    </div>
    <div>
    </div>
</div>

<span>Bill meets with students at North-Grand High School in Chicago.</span>


<p>Then, North-Grand joined the <a href=""https://ncs.uchicago.edu/"">Network for College Success</a>. Armed with data and lessons learned from the other schools in the network, the school changed the way it serves its ninth graders.</p>

<p>If you’re a freshman, your first day now starts with a teacher who helps you with organizational skills, college planning, and how to use your school laptop for assignments. An online portal lets you check your grades every day. Every five weeks, you sit down with a counselor to understand how you’re doing and where to go for help if you need it.</p>

<span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/m6_v3"">
</span>

<p>The school’s approach worked. Last year, 95 percent of North-Grand freshmen were on track for graduation—and the school was ranked one of the best in the city. Many of the other schools in the network have adopted similar programs and experienced similar progress.</p>

<p>Rather than focus on one-size-fits-all solutions, our foundation wants to create opportunities for schools to learn from each other. What worked at North-Grand won’t work everywhere. That’s why it’s important that other schools in other networks share their success stories, too.</p>

<div><span>
Melinda:
</span><span>
The last 20 years have only deepened our commitment to advancing progress on global health and public education. But we’ve also developed a major sense of urgency around two other issues. For Bill, it’s addressing climate change. For me, it’s gender equality.
</span></div>

<p>As we look ahead to the next 20 years, we will be swinging for the fences on these, too.</p>

</div>

<div>
    <div>
        <div>

                    

                    

                    

        </div>
        <div>
            
            <div>Climate</div>
            
        </div>
    </div>
    <div><span>
Bill:
</span><span>
After we started our foundation, Melinda and I began traveling regularly to low-income countries to meet with people and hear firsthand about the issues we were taking on. We’d go to rural villages like Manhiça, in Mozambique, to learn about malaria and visit cities like Lagos, in Nigeria, to meet with local leaders about the HIV crisis.
</span></div>

<p>But even though we were there to hear about health, my mind wasn’t always on diseases. One of the things I noticed on many of those trips was how little electricity there was. After the sun set, entire villages plunged into darkness. I remember seeing unlit streets in Lagos where people huddled around fires they had built in old oil barrels. I also remember thinking we should do something about this.</p>





<div>
    <div>
        <img src="""">
                

                

                

        
    </div>
    <div>
        
        <div>
            
            
            
        </div>
    </div>
    <div>
    </div>
</div>

<span>Men huddle around a fire in Lagos, Nigeria.</span>
  
<p>I didn’t know it then, but that was the beginning of my journey to working on climate change.</p>

<p>That phenomenon we witnessed—which is called “energy poverty”—is a real problem for 860 million people around the world. Our modern world is built on electricity. Without it, you are (quite literally) left in the dark. So, I started talking to experts about the issue and what could be done.</p>

<div>




<div>
    <div>
        <img src="""">
                

                

                

        
    </div>
    <div>
        
        <div>
            
            
            
        </div>
    </div>
    <div>
    </div>
</div>

</div>


<p>Two facts quickly became clear. First, the world would become a richer, healthier, and more equitable place if everyone had reliable access to electricity. Second, we need to find a way to make that happen without contributing to climate change.</p>

<p>That was nearly 14 years ago. Since then, I’ve spent a lot of time and resources exploring new ideas for reducing greenhouse gas emissions and helping people adapt to a changing climate.</p>

<p>When Warren urged Melinda and me to swing for the fences all those years ago, he was talking about the areas our foundation worked on at the time, not climate change. But his advice applies here, too. The world can’t solve a problem like climate change without making big bets.</p>

<p>Tackling climate change is going to demand historic levels of global cooperation, unprecedented amounts of innovation in nearly every sector of the economy, widespread deployment of today’s clean-energy solutions like solar and wind, and a concerted effort to work with the people who are most vulnerable to a warmer world. That won’t happen unless we decide what we’re going to do and how we’re going to do it.</p>

<p>In other words, we need a plan.</p>

<p>The good news is that we already have the ambition to get things done and goals to work toward. The ambition is evidenced by the amazing activism around climate, including last fall’s climate strikes. As for the goals, we can thank the Paris Agreement and all of the countries, cities, and states that have made bold commitments to zero out emissions by 2050.</p>

<p>So, what should the plan to meet that zero-emission goal look like? The answer is as complicated as the problem we’re trying to solve. But the short version breaks down into two buckets: mitigation and adaptation.</p>





<div>
    <div>“So, what should the plan to meet that zero-emission goal look like? The answer is as complicated as the problem we’re trying to solve.”</div>
    <div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Fb.svg""></div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Tw.svg""></div>
    </div>
    
    
    
</div>


<span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/b5_v3"">
</span>

<p><span>Mitigation</span> is all about reducing greenhouse gas emissions. The key to making that happen is a combination of deploying the things that work now and lots of innovation to create and scale the technologies we still need.</p>

<p>When people talk about solving climate change, they usually focus on reducing emissions—which is a good thing! We need zero-carbon alternatives in every sector of the economy, many of which don’t exist yet. Mitigation is, by far, the biggest challenge we need to figure out, and it’s great to see so much energy being put into how to zero out emissions. (I’m also hopeful that the innovation being done in this space will help provide electricity to more people.)</p>

<p>But solving climate change will require more than just mitigation. We also need to take on <span>adaptation</span>.</p>

<span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/m7_v3"">
</span>

<p>People all over the world are already being affected by a warmer world. Those impacts will only get worse in the years to come. The cruel irony is that the world’s poorest people, who contribute the least to climate change, will suffer the worst.</p>

<p>No one will be hit harder than subsistence farmers, who rely on the food they grow to feed their families and already live on the edge of survival. They don’t have the resources to withstand more droughts or floods, a disease outbreak among their herds, or new pests devouring their harvests. At 4 degrees Celsius of warming, most of sub-Saharan Africa could see the growing season shrink by 20 percent or more —and that’s just an average. In areas with severe droughts, the growing season could get cut even shorter.</p>





<div>
    <div>
        <img src="""">
                

                

                

        
    </div>
    <div>
        
        <div>
            
            
            
        </div>
    </div>
    <div>
    </div>
</div>

<span>Smallholder farmers like this woman will be the hardest hit by climate change.</span>
<span>Smallholder farmers like this woman will be the hardest hit by climate change.</span>


<p>The result will be less food, for both the farmers themselves and others who rely on the crops they grow and sell. More kids will suffer from malnutrition, and the already enormous inequity between the rich and poor will get even worse.</p>

<span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/m8_v3"">
</span>

<p>The <a href=""https://gca.org/global-commission-on-adaptation/adapt-our-world"">Global Commission on Adaptation</a> (which I’m co-chair of) recently released a report outlining steps that governments can take to support farmers in the decades to come. I’m also hopeful that our foundation’s work on agriculture will play a key role in helping farmers withstand climate change. Over a decade ago, we began funding research into drought- and flood-tolerant varieties of staple crops like maize and rice. These new varieties are already helping farmers grow more food in some parts of Africa and India, and more climate-smart crop options will become available in more places in the years to come.</p>

<p>But even if we succeed in increasing crop yields, the reality is that climate change will make it harder for many people to get the nutrition they need—which will, in turn, make them more susceptible to disease.</p>





<div>
    <div>“The best thing we can do to help people in poor countries adapt to climate change is make sure they’re healthy enough to survive it.”</div>
    <div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Fb.svg""></div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Tw.svg""></div>
    </div>
    
    
    
</div>


<p>The best thing we can do to help people in poor countries adapt to climate change is make sure they’re healthy enough to survive it. We need to reduce the number of children who become malnourished <i>and</i> improve the odds that people who do suffer from malnutrition survive. That means making sure that people have access not only to the nutrients they need but also to proven interventions like vaccines, drugs, and diagnostics.</p>

<span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/m9_v3"">
</span>

<p>Organizations like <a href=""http://www.gavi.org/"">Gavi</a> and the <a href=""http://www.theglobalfund.org/"">Global Fund</a> are going to play a big role in this by improving health in the most vulnerable places. If we’re going to prevent a climate disaster, climate-specific interventions and solutions aren’t enough. We need to be thinking about the indirect effects, too, like how a warmer planet will affect global health.</p> 

<p>Climate change is one of the most difficult challenges the world has ever taken on. But I believe we <i>can</i> avoid a climate catastrophe if we take steps now to reduce emissions and find ways to adapt to a warmer world.</p>

</div>

<div>
    <div>
        <div>

                    

                    

                    

        </div>
        <div>
            
            <div>Gender</div>
            
        </div>
    </div>
    <div><span>
Melinda:
</span><span>
In addition to the foundation’s 20th anniversary, this year marks another milestone I’ve been thinking about a lot lately: the 25th anniversary of the Beijing World Conference on Women. (If that name doesn’t ring a bell, you may know it as the event where Hillary Clinton famously declared that “Human rights are women’s rights, and women’s rights are human rights.”)
</span></div>

<p>I remember reading about the conference and feeling that the world had planted an important stake in the ground for women. But it took years before I recognized how gender equality would fit into my own work.</p>



<p>After Bill and I started the foundation, I began spending time with women in the world’s poorest places. I wrote a lot about those trips in my book, <a href=""https://www.evoke.org/momentoflift""><i>The Moment of Lift</i></a>, because they changed everything for me.</p>

<p>I met a woman who asked me to take her newborn home with me because she couldn’t imagine how she could afford to take care of him. I met sex workers in Thailand who helped me understand that if I had been born in their place, I, too, would do whatever it took to feed my family. I met a community health volunteer in Ethiopia who told me she once spent the night in a hole in the ground rather than returning to her abusive husband—when she was 10 years old.</p>





<div>
    <div>“The data is unequivocal: No matter where in the world you are born, your life will be harder if you are born a girl.”</div>
    <div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Fb.svg""></div>
        <div><img src=""https://www.gatesnotes.com/img/TGN_badge_TransOutlines_C_Tw.svg""></div>
    </div>
    
    
    
</div>


<p>Each one of these women represents millions more. And what makes their stories even harder to bear is the knowledge that, unless we take action, they are stories that are destined to repeat themselves. Because if there’s one thing the world has learned over these last 25 years, it’s that these problems are not going away on their own.</p>


<p>The data is unequivocal: No matter where in the world you are born, your life will be harder if you are born a girl.</p>

<p>In developing countries, the experiences of boys and girls start dramatically diverging in adolescence. The average girl in sub-Saharan Africa ends her education with two fewer years of schooling than the average boy. One in five girls is married before her 18th birthday, trapping her on the wrong side of a power imbalance even within her own home.</p>
 
<div>




<div>
    <div>
        <img src="""">
                

                

                

        
    </div>
    <div>
        
        <div>
            
            
            
        </div>
    </div>
    <div>
    </div>
</div>

</div>


<p>Meanwhile, in high-income countries, gender inequality tends to be most visible in the workplace. Even though women in the U.S. earn college and graduate degrees at higher rates than men, they tend to be concentrated in certain majors and are often channeled into less lucrative jobs. Men are 70 percent more likely to be executives than women of the same age. These numbers are even worse for women of color, who are doubly marginalized by the combined forces of sexism and racism.</p>

<p>The reason the pace of progress for women and girls has been so glacial is no mystery. It’s the direct result of the fact that—despite the valiant efforts of activists, advocates, and feminist movements—the world has refused to make gender equality a priority. Global leaders simply have not yet made the political and financial commitments necessary to drive real change.</p>

<p>When the world comes together to mark the 25th anniversary of the Beijing Conference at the <a href=""https://www.unwomen.org/en/get-involved/beijing-plus-25/generation-equality-forum"">Generation Equality Forum</a> later this year, it will, I hope, do a lot to generate energy and attention around gender equality. But this time, we need to ensure that our energy and attention are converted into action.</p>

<span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/b6_v3"">
</span>

<p>If we miss another opportunity, if we let the spotlight sputter out again, we risk contributing to a dangerous narrative that inequality between men and women is inevitable. We need to be loud and clear that the reason these problems look unsolvable is that <i>we’ve never put the necessary effort into solving them</i>.</p>

<p>To make this time different, we need to make bold attempts at new solutions that will dismantle inequality by pulling three levers simultaneously.</p>

<p>First is fast-tracking women in leadership positions in critical sectors like government, technology, finance, and health. When more women have a voice in the rooms where decisions are made, more of those decisions will benefit all of us.</p>





<div>
    <div>
        <img src="""">
                

                

                

        
    </div>
    <div>
        
        <div>
            
            
            
        </div>
    </div>
    <div>
    </div>
</div>

<span>Melinda visits the Girls Garage in San Francisco, where girls ages 9–17 learn how to build and
design their own projects.</span>
<span>Melinda visits the Girls Garage in San Francisco, where girls ages 9–17 learn how to build and
design their own projects.</span>


<p>But we can’t stop at top-down change or focus only on women in some fields. We also need to bring down the barriers that women of all backgrounds encounter in their everyday lives. For example, the fact that there’s an estimated 27 percent gap in workforce participation between men and women around the world. Or that our economies are built on the back of women’s unpaid labor. Or that, globally, one in three women is the victim of gender-based violence, one of the most common human rights abuses on the planet. Each one of these barriers makes it harder for a woman to achieve her dreams for herself or contribute her talents and ideas to her community.</p>

<span>
<img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/m11_v3"">
</span>

<p>Lastly, because gender inequality is an issue that touches almost every aspect of society, any response must be broad-based, too. We need to be deliberate about galvanizing a wide range of partners to play a role in changing society’s norms and expectations—not just the activists and advocates who are already leading these conversations, but consumers, shareholders, faith leaders, entertainers, fathers, and husbands.</p>

<p>I’ll admit that when I first started speaking publicly about gender equality, it felt like its own risk. I was deeply aware that our foundation was a latecomer to the issue. I worried about holding my own against the experts and wondered if I was the right messenger for the cause. But I now know that progress depends on all of us speaking up.</p>

<p>My journey as a public advocate began with family planning. There are over 200 million women in developing countries who do not want to get pregnant but are not using modern contraceptives. When women are able to time and space their pregnancies, they are more likely to stay in school, earn an income, and give each of their children the care they need to thrive.</p>

<p>In addition to stepping up our commitments to family planning, I directed our foundation to develop strategies that prioritize gender equality. Over the past several years, we’ve invested to close data gaps, strengthen advocacy, and support women’s economic empowerment.</p>

<p>I’ve also been working to advance women’s power and influence in the U.S. through a company I started called <a href=""https://www.pivotalventures.org/"">Pivotal Ventures</a>. Last October, I announced that Pivotal Ventures will commit $1 billion to accelerate gender equality in the U.S. over the next decade, an investment that I hope is seen as a vote of confidence in the experts and advocates already working on these issues—and an invitation for other philanthropists to make major commitments of their own, both in the U.S. and around the world.</p>

<p>As the anniversary of the Beijing Conference approaches, it’s time for government leaders, business executives, philanthropists, and individuals from every walk of life to take concrete steps to put our aspirations for a more equal world into action.</p>


<p>My message is simple: <span>Equality can’t wait.</span></p>



<div>
    <div>
        <div class=""video_frame""><iframe src=""https://www.youtube.com/embed/sKs1a261YMM?rel=0&amp;modestbranding=1&amp;playsinline=1&amp;color=white&amp;showinfo=0""></iframe></div>
    </div>
</div>

</div>

<div>
    <div>
        <div>

                    

                    

                    

        </div>
        <div>
            
            
            
        </div>
    </div>
    <span>Looking ahead</span>

<p>When Bill’s mom spoke at our wedding, she said something we’ll always remember: “Your lifetime together will, in the end, be a verdict on your recognition of the extraordinary obligations which accompany extraordinary resources.” Over the last 20 years, we’ve worked to live up to those obligations through our foundation.</p>





<div>
    <div>
        <img src="""">
                

                

                

        
    </div>
    <div>
        
        <div>
            
            
            
        </div>
    </div>
    <div>
    </div>
</div>

<span>Bill’s mother gives a speech on our wedding day.</span>

<p>When we first started this work, we were optimistic about the power of innovation to drive progress—and excited about the role we could play by taking risks to unlock it.</p>

<p>Twenty years later, we’re just as optimistic—and we’re still swinging for the fences. But we now have a much deeper understanding of how important it is to ensure that innovation is distributed equitably. If only some people in some places are benefitting from new advances, then others are falling even further behind.</p>

<p>Our role as philanthropists is not only to take risks that support innovation but to work with our partners to overcome the challenges of scale in delivering it. We believe that progress should benefit everyone, everywhere.</p>

<p>That’s why we’ve been at this work for the last two decades. And that’s why we hope to continue it for many decades ahead.</p>

<div><img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/BillAndMelindaSig_v04""></div>
<p> </p>

 




<span>Dedication</span>

<div>
    <div><img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/20190108-AL2020-GA1700268-001""></div>
    We’re dedicating this year’s letter to all the people who have made our foundation’s work possible.
<span> </span>
Our first thank-you goes to our colleagues at the foundation—and to the thousands of hardworking people who have passed through its doors over the last 20 years. You are a truly world-class group of advocates and experts, and the foundation’s success is a tribute to your ability to cultivate strong relationships with our partners around the world. Many of you were at this work long before we were. Many of you will be at it long after we’re gone. We are grateful for the chance to learn from you.
<span> </span>
We also want to thank our partners: the governments, organizations, and individuals on the front lines of progress. We have gained much from your insights and expertise, and we draw inspiration from the courage many of you demonstrate as you take much bigger risks than we do to create a better future for your countries and communities.
<span> </span>

<span><img src=""https://www.gatesnotes.com/-/media/Images/Articles/About-Bill-Gates/Annual-Letter/2020-Annual-Letter/20190108-AL2020-GA1700268-001""></span>
<span>Bill’s dad celebrates his birthday with foundation staff.</span>

Finally, there is one person in particular who kept coming up as we reflected on these last 20 years, and that’s Bill Gates, Sr. Without you, the foundation wouldn’t be what it is, and we wouldn’t be who we are. In the spirit of baseball metaphors, you are truly in a league of your own.
</div>





</div>

                        </div>&#13;
                        ");
        }


    }
}
