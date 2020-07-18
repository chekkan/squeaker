test-all: test-u test-a

test-u:
	dotnet test ./test/Squeaker.UnitTests

test-a:
	dotnet test ./test/Squeaker.AcceptanceTests
