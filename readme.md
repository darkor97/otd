## Pre-requisites

 - To run the solution docker is necessary. If not present on machine download from [here](https://www.docker.com/products/docker-desktop/).
 - The target OS is linux, check that the engine is using the correct containers.

## Run application and interact with it
 1. Open a console of your choice and navigate to the root folder (where the docker-compose.yaml file is located)
 2. Enter command `docker-compose -p otd up`
 3. Open two more console windows
 4. In one window enter command `docker attach handler` to interact with the odds handler application. Type anything to get command instructions.
 5. In the other window enter command `docker attach punter` to interact with the punter application. Onces first publish has been done from the handler app, the odds will appear.
 6. Once finished with the app run stop using CTRL+C
 7. Enter command `docker-compose -p otd down` to cleanup resources
    - Run `docker image rm handler-console` 
    - Run `docker image rm punter-console`
    - Run `docker image rm mongodb`, if only used for this project
    - Run `docker image rm rabbitmq`, if only used for this project

## Run 
### Unit tests
Find tests in the test explorer from the group `Handler.Tests.Application` and run it.
Alternatively navigate to test folder location  ./{Project-Root}/Handler/Tests and run `dotnet test --filter "Handler.Tests.Application"`
### Integration tests
__Ensure mongo docker image is running for integration tests.__
Find tests in the test explorer from the group  `Handler.Tests.Integration` and run it.
Alternatively navigate to test folder location  ./{Project-Root}/Handler/Tests and run `dotnet test --filter "Handler.Tests.Integration"`

If using Visual Studio and docker container for mongodb is running, press shortcut keys for running all tests __(default CTRL+R, A) (TestExplorer.RunAllTests)__
Alternatively navigate to test folder location  ./{Project-Root}/Handler/Tests and run `dotnet test`