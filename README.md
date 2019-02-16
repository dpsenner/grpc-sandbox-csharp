# grpc-sandbox-csharp

A csharp sandbox for GRPC.

## Usage

1. Start the server

    ```shell
    $ dotnet Server/bin/Debug/netcoreapp2.1/Server.dll run --listen 0.0.0.0:50051
    Press CTRL+C to cancel.
    ```

1. Stream 1GiB random bytes in chunks of 1MiB from the server to the client:

    ```shell
    $ dotnet Client/bin/Debug/netcoreapp2.1/Client.dll stream-bytes --chunk-size 1000000 --total-chunks 1000
    Press CTRL+C to cancel.
    Connecting to grpc host localhost:50051 ..
    Connection established to grpc host localhost:50051
    Transferred 1000000000 bytes in 1654,2158ms (576,098536183794 MBytes/second)
    ```
