init:
	git config core.hooksPath .githooks

test-all: test-u test-a

test-u:
	dotnet test ./test/Squeaker.UnitTests
	dotnet test ./test/Squeaker.Api.UnitTests

test-a:
	dotnet test ./test/Squeaker.AcceptanceTests
