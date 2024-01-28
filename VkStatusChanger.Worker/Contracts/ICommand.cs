namespace VkStatusChanger.Worker.Contracts;

internal interface ICommand { }
internal interface ICommand<T> : ICommand
{
    public Task<string> Execute(T request);
}
