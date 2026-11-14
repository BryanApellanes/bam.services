using System.Reflection;
using Bam.CoreServices.ServiceRegistration.Data;
using Bam.DependencyInjection;
using Bam.Test;

namespace Bam.Services.Tests;

/// <summary>
/// Service identity for a type from an assembly that has no file (loaded from bytes, as generated
/// decorators are) falls back to the assembly's name instead of throwing (bam.services#4).
/// </summary>
[UnitTestMenu("Service identity for a file-less assembly should")]
public class FileLessAssemblyIdentityShould : UnitTestMenuContainer
{
    [UnitTest]
    public void IdentifyATypeFromAnAssemblyLoadedFromBytes()
    {
        When.A<Assembly>("supplies a service type from an assembly loaded from bytes",
            LoadTestAssemblyFromBytes,
            (bytesLoaded) =>
            {
                Type bytesLoadedType = bytesLoaded.GetType(typeof(TestServiceRegistryContainer).FullName!, true)!;
                ServiceTypeIdentifier fromBytes = ServiceTypeIdentifier.FromType(bytesLoadedType);
                ServiceTypeIdentifier fromFile = ServiceTypeIdentifier.FromType(typeof(TestServiceRegistryContainer));
                return new IdentityOutcome(fromBytes, fromFile, bytesLoaded.FullName!, bytesLoaded.ManifestModule.ModuleVersionId.ToString().Sha1(), new FileInfo(typeof(TestServiceRegistryContainer).Assembly.Location));
            })
        .TheTest
        .ShouldPass<IdentityOutcome>((because, _, outcome) =>
        {
            because.ItsTrue("the byte-loaded type is named by its assembly's full name", outcome.FromBytes.AssemblyName!.Equals(outcome.BytesLoadedFullName));
            because.ItsTrue("the byte-loaded type is hashed by its module version id", outcome.FromBytes.AssemblyFileHash!.Equals(outcome.ModuleVersionIdHash));
            because.ItsTrue("the file-loaded type is still named by its file", outcome.FromFile.AssemblyName!.Equals(outcome.FileLoadedFile.Name));
            because.ItsTrue("the file-loaded type is still hashed by its file", outcome.FromFile.AssemblyFileHash!.Equals(outcome.FileLoadedFile.Sha1()));
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void NameALoaderFromAnAssemblyLoadedFromBytes()
    {
        When.A<Assembly>("holds a service registry container and was loaded from bytes",
            LoadTestAssemblyFromBytes,
            (bytesLoaded) => new ServiceRegistryLoaderDescriptor(bytesLoaded))
        .TheTest
        .ShouldPass<ServiceRegistryLoaderDescriptor>((because, _, descriptor) =>
        {
            because.ItsTrue("the default name uses the assembly's file name", descriptor.Name.Equals($"bam.services.tests.dll_{nameof(TestServiceRegistryContainer)}"));
            because.ItsTrue("the loader type is recorded", descriptor.LoaderType.Equals(typeof(TestServiceRegistryContainer).FullName));
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    private static Assembly LoadTestAssemblyFromBytes()
    {
        return Assembly.Load(File.ReadAllBytes(typeof(FileLessAssemblyIdentityShould).Assembly.Location));
    }

    private sealed record IdentityOutcome(ServiceTypeIdentifier FromBytes, ServiceTypeIdentifier FromFile, string BytesLoadedFullName, string ModuleVersionIdHash, FileInfo FileLoadedFile);
}

/// <summary>
/// A registry container for the loader-descriptor test. An empty registry name makes the descriptor
/// fall back to the assembly-based default name.
/// </summary>
[ServiceRegistryContainer]
public class TestServiceRegistryContainer
{
    [ServiceRegistryLoader("")]
    public static ServiceRegistry Load()
    {
        return new ServiceRegistry();
    }
}
