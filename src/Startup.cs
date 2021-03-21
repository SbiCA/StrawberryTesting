using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace StrawberryTesting
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType(d => d.Name("Query"))
                .AddTypeExtension<ReaderQueries>()
                .AddMutationType(d => d.Name("Mutation"))
                .AddTypeExtension<BloggerMutations>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            // TODO add authN + authZ

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }

    public record BlogPost(string Title, string Description, string Content, DateTime Published, IEnumerable<string> Tags, Author Owner);
    public record Author (string UserId, string Name);
    
    [ExtendObjectType("Mutation")]
    public class BloggerMutations
    {
        public BlogPost CreateBlogPost(BlogInput input)
        {
            // TODO check role "blogger"
            throw new NotImplementedException();
        }
        
        public BlogPost UpdateBlogPost(BlogInput input)
        {
            // TODO check owner ship
            throw new NotImplementedException();
        }

        public BlogPost Publish(string blogId)
        {
            // TODO check owner ship
            throw new NotImplementedException();
        }
        
    }

    public record BlogInput(string Title, string Description, string Content, IEnumerable<string> Tags);

    [ExtendObjectType("Query")]
    public class ReaderQueries
    {
        public IEnumerable<BlogPost> GetPosts()
        {
            var simon = new Author("1", "simone@the-blogger.com");
            return new List<BlogPost>
            {
              
                new("My first blog", "Its so nice", "here is some html", new DateTime(2020, 8, 1), new List<string>
                {
                    "Geek"
                }, simon)
            };
        }
    }
}