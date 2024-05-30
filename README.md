# OnMapper

<p align="center">
  <img src='assets/OnMapper.png' alt='Icon'>
</p>


**OnMapper**  is aimed to save time and efforts to map Data Transfer Objects (DTOs) to and from database models for your application and keep strong enough separation of the presentation layer from the data-access layer. It fully exploits the Lightweight Reflection mechanism and its ability to perform operations based on the names or display names of the properties to achieve a lower level of code redundancy that is characteristic of similar operations.

**The rationale as to why people should embrace this package is based on the following considerations:**

- Separation of Concerns: With this package, you can easily provide separation between the database models and the DTOs for enhanced modularity and easy code maintenance. This separation is crucial in minimizing the level of coupling that occurs between the different layers of your application.  
  
- Ease of Mapping: This package helps in mapping between two or more objects hence making it easy to work with different objects in a program. It can look at properties by their names or display names and only requires minimal prerequisite mapping code compared to writing the same code manually.  
  
- Flexibility: It also supports higher-order mappings, that is, mappings of objects and collections one within another. It can perform simple conversions of certain data types which might include a conversion from DateTime to long (ticks), and can work with lists and other forms of generics.  
  
- Error Handling: The package comes with the necessary error handling mechanisms that can be helpful while mapping, in case it fails, it provides the necessary error messages, which are helpful in data validation and debugging.  
  
- Asynchronous Support: All the methods in the package are created based on asynchronous programming, which is used in the modern application, to support Good-Enough-Computing.



### Installation

OnMapper is available on [NuGet](https://www.nuget.org/packages/OnTube). Install the provider package. See the in the docs for additional downloads.

```sh
dotnet tool install --global OnMapper --version 1.1.9
```


## Usage

Here's a simple example demonstrating how to use the package to separate concerns between database models and DTOs


#### Database Models

```csharp
public class BlogPost 
{ 
	public int Id { get; set; } 
	public string Title { get; set; } 
	public string Content { get; set; } 
	public ICollection<Category> Categories { get; set; } = new List<Category>(); 
} 

public class Category 
{ 
	public int Id { get; set; } 
	public string Name { get; set; } 
}

```


####  DTOs

```csharp
public class BlogPostRequestDTO
{
    public string Title { get; set; }
    public string Content { get; set; }
    public List<int> Categories { get; set; }
}

public class BlogPostResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<CategoryResponseDTO> Categories { get; set; } = new    List<CategoryResponseDTO>();
}

public class CategoryResponseDTO
{
	public string Name { get; set; }
    public string UrlHandle { get; set; }  
}

```


#### BlogPostController

```csharp

  
 [Route("api/[controller]")]
 [ApiController]	
 public class BlogPostController : ControllerBase
{
	 private readonly IBlogPostRepository _repository;
	 private readonly ICategoryRepository _caterepository;
     private readonly OnMapping _mapper;
	 
	 public BlogPostController
	 (
	     IBlogPostRepository repository, 
	     ICategoryRepository caterepository, 
	     OnMapping mapper
	 )
	{
	    _repository = repository;
	    _caterepository = caterepository;
	    _mapper = mapper;
	}

	
    // GET: api/<BlogPost>
	[HttpGet]
	public async Task<Result<IEnumerable<BlogPostResponseDTO>>> Get()
	{
	
	    // Get all Blogpost
	    var res = await _repository.GetAllAsync();
	
	    if (res is null)
	    {
        	return await              Result<IEnumerable<BlogPostResponseDTO>>.FaildAsync(false, "No pages");
	    }
	
	
	    //Mapping from model to response
	    var responseList = await _mapper.MapCollection<BlogPost,    BlogPostResponseDTO>(res.Data);
	
	    return await Result<IEnumerable<BlogPostResponseDTO>>.SuccessAsync(responseList.Data, "Viewd Successfully", true);
	}

   // POST api/<BlogPost>
	[HttpPost]
 	public async Task<Result<BlogPostResponseDTO>> Create([FromBody]     BlogPostRequestDTO request)
	{
	
       var blogpostResult = await _mapper.Map<BlogPostRequestDTO, BlogPost>      (request);
	
	    if (!blogpostResult.IsSuccess)
	    {
	        return await Result<BlogPostResponseDTO>.FaildAsync(false, "Mapping Failed");
	    }
	
	    var blogpost = blogpostResult.Data;
	
	    blogpost.Categories.Clear();
	
	    foreach (var categoryId in request.Categories)
	    {
	        var exisitingCategory = await _caterepository.GetByIdAsync(categoryId);
	
	        if (exisitingCategory is not null)
	        {
	            blogpost.Categories.Add(exisitingCategory.Data);
	        }
	    }
	
	    var add = await _repository.CreateAsync(blogpost);
	
	    //Mapping the model to Response
	    var responseResult = await _mapper.Map<BlogPost, BlogPostResponseDTO>   (blogpost);
	
	    return await Result<BlogPostResponseDTO>.SuccessAsync(responseResult.Data,         "Added Successfullly", true);
	}

}


```


#### program.cs

```csharp
builder.Services.AddScoped<OnMapping>();
```


## Important Notice: Clearing Nested Lists After Mapping

#### Warning 
- When using the mapping function provided by this package, be aware that any nested lists within your models will be instantiated with null elements. To avoid issues, you must clear these lists before adding any items to them.

- This behavior is due to how the mapping function handles nested objects and collections. When a list property is mapped, the function creates a list with the appropriate number of elements, but these elements are initialized to null. You must clear these lists before populating them with actual data to avoid null reference exceptions or unintended behavior.

   ####  Example: BlogPost and Categories
	- Consider the following example where you map a `BlogPostRequestDTO` to a `BlogPost` model. The `BlogPost` model contains a list of `Category` objects.


<br >

   #####  BlogPostRequestDTO
   
```csharp
public class BlogPostRequestDTO
{
    public string Title { get; set; }
    public string Content { get; set; }
    public List<int> Categories { get; set; }
}
```

####       BlogPost Model

```csharp
public class BlogPost
{
    public int Id { get; set; } 
    public string Title { get; set; } 
    public string Content { get; set; } 
	public ICollection<Category> Categories { get; set; } = new List<Category>();
}
```
####       Category Model

```csharp
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

### Mapping and Clearing Lists

When mapping a `BlogPostRequestDTO` to a `BlogPost`, the `Categories` list in the `BlogPost` model will be instantiated with null elements. To handle this, you must clear the `Categories` list before adding any items.

#### Example Code

```csharp
// POST api/<BlogPost>
[HttpPost]
public async Task<Result<BlogPostResponseDTO>> Create([FromBody] BlogPostRequestDTO request)
{
    var blogpostResult = await _mapper.Map<BlogPostRequestDTO, BlogPost>(request);

    if (!blogpostResult.IsSuccess)
    {
        return await Result<BlogPostResponseDTO>.FaildAsync(false, "Mapping Failed");
    }

    var blogpost = blogpostResult.Data;

    // Clear any existing categories from the blogpost
    blogpost.Categories.Clear();

    foreach (var categoryId in request.Categories)
    {
        var existingCategoryResult = await _categoryRepository.GetByIdAsync(categoryId);

        if (existingCategoryResult.IsSuccess && existingCategoryResult.Data != null)
        {
            blogpost.Categories.Add(existingCategoryResult.Data);
        }
    }

    var addResult = await _repository.CreateAsync(blogpost);

    var responseResult = await _mapper.Map<BlogPost, BlogPostResponseDTO>(blogpost);

    return await Result<BlogPostResponseDTO>.SuccessAsync(responseResult.Data, "Added Successfully", true);
}

```

### Summary

To ensure proper functionality, always clear any nested lists in your models after mapping and before adding new items. This step is crucial to avoid null references and to ensure that your lists contain the correct data.

By including this notice in your documentation and providing a clear example, you will help developers understand the necessary steps and avoid common pitfalls when using your package.



## 😁 Contributing

Pull requests are welcome but before that please open an issue first to discuss what you want to change 🤙.
please follow me for more C# blogs [linkedin](https://www.linkedin.com/in/osama-dammag-%F0%9F%87%B5%F0%9F%87%B8-b40739221/)


## 📎 License

This project is licensed under the [MIT License](https://github.com/OND10/OnMapper/blob/master/LICENSE.txt).

