namespace HomeStream.Application.Dtos;

public class ResponseDto<T>(T data)
{
    public T Data { get; set; } = data;
}
