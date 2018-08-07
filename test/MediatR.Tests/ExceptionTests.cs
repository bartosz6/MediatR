namespace MediatR.Tests
{
    using System;
    using System.Threading.Tasks;
    using Shouldly;
    using StructureMap;
    using Xunit;
    using DummyClasses;

    public class ExceptionTests
    {
        protected IMediator Mediator;

        public class HandlerNotRegistered : ExceptionTests
        {
            public HandlerNotRegistered()
            {
                var container = new Container(cfg =>
                {
                    cfg.For<ServiceFactory>().Use<ServiceFactory>(ctx => ctx.GetInstance);
                    cfg.For<IMediator>().Use<Mediator>();
                });
                Mediator = container.GetInstance<IMediator>();
            }

            [Fact]
            public async Task Should_throw_for_send()
            {
                await Should.ThrowAsync<InvalidOperationException>(async () => await Mediator.Send(new Ping()));
            }

            [Fact]
            public async Task Should_throw_for_void_send()
            {
                await Should.ThrowAsync<InvalidOperationException>(async () => await Mediator.Send(new VoidPing()));
            }

            [Fact]
            public async Task Should_not_throw_for_publish()
            {
                Exception ex = null;
                try
                {
                    await Mediator.Publish(new Pinged());
                }
                catch (Exception e)
                {
                    ex = e;
                }

                ex.ShouldBeNull();
            }

            [Fact]
            public async Task Should_throw_for_async_send()
            {
                await Should.ThrowAsync<InvalidOperationException>(async () => await Mediator.Send(new AsyncPing()));
            }

            [Fact]
            public async Task Should_throw_for_async_void_send()
            {
                await Should.ThrowAsync<InvalidOperationException>(
                    async () => await Mediator.Send(new AsyncVoidPing()));
            }

            [Fact]
            public async Task Should_not_throw_for_async_publish()
            {
                Exception ex = null;
                try
                {
                    await Mediator.Publish(new AsyncPinged());
                }
                catch (Exception e)
                {
                    ex = e;
                }

                ex.ShouldBeNull();
            }
        }

        public class ArgumentValidation : ExceptionTests
        {
            public ArgumentValidation()
            {
                var container = new Container(cfg =>
                {
                    cfg.Scan(scanner =>
                    {
                        scanner.AssemblyContainingType(typeof(NullPing));
                        scanner.IncludeNamespaceContainingType<Ping>();
                        scanner.WithDefaultConventions();
                        scanner.AddAllTypesOf(typeof(IRequestHandler<,>));
                    });
                    cfg.For<ServiceFactory>().Use<ServiceFactory>(ctx => ctx.GetInstance);
                    cfg.For<IMediator>().Use<Mediator>();
                });
                Mediator = container.GetInstance<IMediator>();
            }

            [Fact]
            public async Task Should_throw_argument_exception_for_send_when_request_is_null()
            {
                NullPing request = null;

                await Should.ThrowAsync<ArgumentNullException>(async () => await Mediator.Send(request));
            }

            [Fact]
            public async Task Should_throw_argument_exception_for_void_send_when_request_is_null()
            {
                VoidNullPing request = null;

                await Should.ThrowAsync<ArgumentNullException>(async () => await Mediator.Send(request));
            }

            [Fact]
            public async Task Should_throw_argument_exception_for_publish_when_request_is_null()
            {
                NullPinged notification = null;

                await Should.ThrowAsync<ArgumentNullException>(async () => await Mediator.Publish(notification));
            }

            [Fact]
            public async Task Should_throw_argument_exception_for_publish_when_request_is_null_object()
            {
                object notification = null;

                await Should.ThrowAsync<ArgumentNullException>(async () => await Mediator.Publish(notification));
            }

            [Fact]
            public async Task Should_throw_argument_exception_for_publish_when_request_is_not_notification()
            {
                object notification = "totally not notification";

                await Should.ThrowAsync<ArgumentException>(async () => await Mediator.Publish(notification));
            }
        }
    }
}