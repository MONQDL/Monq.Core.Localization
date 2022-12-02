# Monq.Core.Localization

ASP.NET Core library for HTTP request localization.

## Localization with resource files (.resx)

Add a dummy class (implementation of library interface `ILocalizationResource`) for shared resources.

Note, that resource files naming depends on class location in the solution (e.g., class is located in the directory `../Localization/LocalizationResource.cs`, so resource files should be named as `Localization.LocalizationResource.xx-XX.resx`). More info: <https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-6.0#resource-file-naming>.

```c#
builder.Services.AddResourceLocalization<LocalizationResource>();
```

You can also optionally configure resource localization (see <https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.localization.localizationoptions?view=dotnet-plat-ext-6.0>). Defaults: `ResourcePath="Resources"`.

```c#
builder.Services.AddResourceLocalization<LocalizationResource>(opt => opt.ResourcePath = "MyResources");
```

## Localization with EF DbContext

Inherit your `DbContext` from library class `LocalizationDbContext`.

```c#
builder.Services.AddDbContextLocalization<MyDbContext>();
```

## Middleware

Middleware configures supported cultures and default request culture.

Supported cultures:

- en-US (default)
- ru-RU

```c#
app.UseMonqLocalization();
```

Options are completely overridable (see <https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.requestlocalizationoptions?view=aspnetcore-6.0>).

```c#
app.UserMonqLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture("en-US")
});
```

## Additional services

`ILocalizedModelBuilder` provides localization for class properties. For this purpose `LocalizablePropertyAttribute` should be used.

Supported localizable property types:

- `string`
- `IEnumerable<string>`

You can also add attributes to properties that are represented as an object or a collection of objects containing localizable properties.

## Examples

### Services registration

Resource files localization.

```c#
public class LocalizationResource : ILocalizationResource
{
}
```

```c#
builder.Services
    .AddControllers()
    // For data annotation attributes localization.
    .AddMonqDataAnnotationsLocalization<LocalizationResource>();

builder.Services.AddResourceLocalization<LocalizationResource>();

var app = builder.Build();

app.UseMonqLocalization();
app.MapControllers();

app.Run();
```

DbContext localization.

```c#
public class MyDbContext: LocalizationDbContext
{
    public DbSet<MyModel> MyModels => Set<MyModel>();

    public MyDbContext(DbContextOptions<MyDbContextOptions> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ...

        modelBuilder.ConfigureLocalizationContext(); // Configure localization tables.

        SeedData(modelBuilder);
    }

    static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lang>(entity =>
            entity.HasData(new[]
            {
                new Lang { Id = "en-US" },
                new Lang { Id = "ru-RU" },
            }));
        modelBuilder.Entity<Resource>(entity =>
            entity.HasData(new[]
            {
                new Resource { LangId = "en-US", Key = "NameRequired", Value = "Name is required." },
                new Resource { LangId = "en-US", Key = "WrongId", Value = "Wrong Id." },
                new Resource { LangId = "en-US", Key = "default-name", Value = "Name by default" },
                new Resource { LangId = "ru-RU", Key = "NameRequired", Value = "Требуется указать название." },
                new Resource { LangId = "ru-RU", Key = "WrongId", Value = "Некорректное значение идентификатора." },
                new Resource { LangId = "ru-RU", Key = "default-name", Value = "Название по умолчанию" },
            }));
    }
}
```

```c#
builder.Services
    .AddControllers()
    // For data annotation attributes localization.
    .AddMonqDataAnnotationsLocalization<LocalizationResource>();

builder.Services.AddDbContext<MyDbContext>();
builder.Services.AddDbContextLocalization<MyDbContext>();

var app = builder.Build();

app.UseMonqLocalization();
app.MapControllers();

app.Run();
```

### Dependecy injection

`.../Resources/LocalizationResource.en-US.resx`

|Name|Value|
|-|-|
|NameRequired|Name is required.|
|WrongId|Wrong Id.|
|default-name|Name by default|

`.../Resources/LocalizationResource.ru-RU.resx`

|Name|Value|
|-|-|
|NameRequired|Требуется указать название.|
|WrongId|Некорректное значение идентификатора.|
|default-name|Название по умолчанию|

```c#
public class MyPostModel
{
    public long Id { get; set; }

    [Required(ErrorMessage = "NameRequired")]
    public string Name { get; set; }
}

public class MyModel
{
    public long Id { get; set; }

    [LocalizableProperty]
    public string Name { get; set; } = "default-name";
}
```

Localization via `IStringLocalizer`.

```c#
public class MyController : ControllerBase
{
    readonly IStringLocalizer _localizer;
    readonly ILocalizedModelBuilder _localizedModelBuilder;

    public MyController(
        IStringLocalizer localizer,
        ILocalizedModelBuilder localizedModelBuilder)
    {
        _localizer = localizer;
        _localizedModelBuilder = localizedModelBuilder;
    }

    [HttpPost]
    public ActionResult<MyModel> MyMethod(
        [FromBody] MyPostModel value)
    {
        if (value.Id < 1)
            return BadRequest(_localizer["WrongId"]);

        .
        .
        .

        var result = _localizedModelBuilder.Build(myModel);
        return Ok(result);
    }
}
```

Resource files localization via autogenerated class.

You should have the following structure:

1. Resource file for default culture **without** culture in name: `../Resources/LocalizationResource.resx`

2. Resource files for non-default cultures named by standard: `../Resources/LocalizationResource.en-US.resx`

3. Make resource files public: set the `Access Modifier` when you open .resx file in Visual Studio or set `Custom Tool` property to `PublicResXFileCodeGenerator`.

```c#
[HttpPost]
public ActionResult<MyModel> MyMethod(
    [FromBody] MyPostModel value)
{
    if (value.Id < 1)
        return BadRequest(Resources.LocalizationResource.WrongId);

    .
    .
    .
}
```

### Tests

To set culture for controller unit tests you should use `CultureInfo` static properties.

As an example for xUnit:

```c#
public class MyTests
{
    public MyTests()
    {
        CultureInfo.CurrentCulture = new("en-US");
        CultureInfo.CurrentUICulture = new("en-US");
    }

    [Fact]
    public void MyTest()
    {
        .
        .
        .
    }
}
```
