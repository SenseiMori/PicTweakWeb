namespace WebPicTweak.Application.Services.ImageServices.RemoveExif.JPG
{
    public class JPGFile
    {
        private readonly HashSet<byte> _markers = new JPGMarkers().markers;

        public async Task<byte[]> GetJPGWithoutAppSegments(string file)
        {
            JPGMarkers jPGMarkers = new JPGMarkers();

            List<byte> cleanImageData = new List<byte>();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            {
                using (BinaryReader binaryReader = new BinaryReader(fs))
                {
                    byte[] buffer = new byte[2];
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        int read = await binaryReader.BaseStream.ReadAsync(buffer);
                        if (buffer[0] == 0xFF && _markers.Contains(buffer[1]))
                        {
                            int appLength = binaryReader.ReadUInt16();
                            int reversBytes = ShiftBytes(appLength);
                            binaryReader.BaseStream.Seek(reversBytes - 2, SeekOrigin.Current);
                        }
                        else if (read == 1)
                        {
                            cleanImageData.Add(buffer[0]);
                        }
                        else
                        {
                            cleanImageData.Add(buffer[0]);
                            cleanImageData.Add(buffer[1]);
                        }
                    }
                }
                return cleanImageData.ToArray();
            }
        }
        public async Task<List<byte>> GetMarkersAppSegment(string file)
        {
            List<byte> markers = new List<byte>();
            JPGMarkers jPGMarkers = new JPGMarkers();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            {
                using (var binaryReader = new BinaryReader(fs))
                {
                    byte[] buffer = new byte[2];
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        int read = await binaryReader.BaseStream.ReadAsync(buffer);
                        if (buffer[0] == 0xFF && _markers.Contains(buffer[1]))
                        {
                            int appLength = binaryReader.ReadUInt16();
                            int reversBytes = ShiftBytes(appLength);
                            markers.Add(buffer[1]);
                        }
                    }
                    return markers;
                }
            }
        }

        public ushort ShiftBytes(int value)
        {
            byte secondByte = (byte)(value & 0xFF);
            int firstByte = value >> 8;
            int result = secondByte << 8 | firstByte;
            return (ushort)result;
        }

    }
    public class JPGMetadataReader
    {
        JPGFile JPGFile { get; set; }

        public JPGMetadataReader()
        {
            JPGFile = new JPGFile();
        }

        public async Task<byte[]> DeleteExifMarkers(string pathToFile) => await JPGFile.GetJPGWithoutAppSegments(pathToFile);

        #region Find Markers
        public async Task<List<byte>> ReadExifFromImage(string file)
        {
            List<byte> markersList = new List<byte>();
            List<byte> data = new List<byte>();
            data = await JPGFile.GetMarkersAppSegment(file);

            foreach (byte marker in data)
            {
                markersList.Add(marker);
            }
            return markersList;
        }
        #endregion
    }
}
