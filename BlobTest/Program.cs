using Azure.Storage.Blobs;

string connectionString = "UseDevelopmentStorage=true";
string containerName = "samples-workitems";
string blobName = "producto.txt";

BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

BlobContainerClient containerClient =
    blobServiceClient.GetBlobContainerClient(containerName);

await containerClient.CreateIfNotExistsAsync();

BlobClient blobClient = containerClient.GetBlobClient(blobName);

string contenido = """
Producto de prueba
Nombre: Cuenta Ahorro
Precio: 100
""";

using MemoryStream stream =
    new MemoryStream(System.Text.Encoding.UTF8.GetBytes(contenido));

await blobClient.UploadAsync(stream, overwrite: true);

Console.WriteLine($"Blob creado correctamente: {blobName}");