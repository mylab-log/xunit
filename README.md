# MyLab.Log.XUnit

Ознакомьтесь с последними изменениями в [журнале изменений](/CHANGELOG.md).

[![NuGet](https://img.shields.io/nuget/v/MyLab.Log.XUnit.svg)](https://www.nuget.org/packages/MyLab.Log.XUnit/)

## Обзор

Инструмент для интеграции средств вывода `XUnit` в подсистему логирования `.NET`. Разработано на платформе `.NET5`.

Для применения в тесте необходимо:

* в конструкторе класса теста получить объект тестового вывода
* сохранить его в поле класса
* в методе теста использовать специальный метод расширения для интеграции

```c#
public class Demo
{
    private ITestOutputHelper _output;

    public Demo(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void DemoOutput()
    {
        var appServices = new ServiceCollection()
            .AddLogging(l => 
                    l.AddXUnit(_output)				// Добавление логирование в тестовый вывод XUnit
                )
            .BuildServiceProvider();

        var logger = appServices.GetRequiredService<ILogger<Demo>>();

        logger.LogInformation("Foo");
    }
}
```

Тестовый вывод:

```
info: UnitTests.Demo[0]
       Foo
```

## Форматирование

Логгер для XUnit использует те же настройки и форматтер сообщений, что и предназначенные для косольного логгера. 

```c#
public class Demo
{
    private ITestOutputHelper _output;

    public Demo(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void DemoFormatting()
    {
        var appServices = new ServiceCollection()
            .AddLogging(l => l.AddXUnit(_output))
            .Configure<ConsoleLoggerOptions>(o => o.FormatterName = ConsoleFormatterNames.Systemd)
            .BuildServiceProvider();

        var logger = appServices.GetRequiredService<ILogger<Demo>>();

        logger.LogInformation("Foo");
    }
}
```

Тестовый вывод, сформированный форматтером под `systemd`:

```
<6>UnitTests.Demo[0] Foo
```

