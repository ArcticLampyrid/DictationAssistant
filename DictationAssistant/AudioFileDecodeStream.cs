using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DictationAssistant
{
    public sealed class AudioFileDecodeStream : Stream
    {
        [DllImport("bass.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int BASS_StreamCreateFile([MarshalAs(UnmanagedType.Bool)] bool mem, string file, long offset, long length, int flags);
        [DllImport("bass.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int BASS_StreamFree(int handle);
        [DllImport("bass.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int BASS_ChannelPlay(int handle, int restart);
        [DllImport("bass.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int BASS_ChannelStop(int handle);
        [DllImport("bass.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int BASS_ChannelIsActive(int handle);
        [DllImport("bass.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int BASS_ChannelSetAttribute(int handle, int attrib, float value);
        [DllImport("bass.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int BASS_ChannelGetInfo(int handle, ref BASS_CHANNELINFO info);
        [DllImport("bass.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int BASS_ChannelGetData(int handle, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] buffer, int length);
        [DllImport("bass.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int BASS_ChannelGetData(int handle, IntPtr buffer, int length);
        [DllImport("bass.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern long BASS_ChannelGetLength(int handle, int mode);
        [StructLayout(LayoutKind.Sequential)]
        public struct BASS_CHANNELINFO
        {
            public int freq;
            public int chans;
            public int flags;
            public int channelType;
            public int origres;
            public int plugin;
            public int sample;
            public int filename;
        }
        private const int BASS_UNICODE = unchecked((int)0x80000000);
        private const int BASS_STREAM_DECODE = 0x200000;
        private const int BASS_SAMPLE_8BITS = 1;
        private const int BASS_POS_BYTE = 0;

        public static PcmStreamWithInfo Create(string url)
        {
            AudioFileDecodeStream PcmStream = new AudioFileDecodeStream(url);
            return new PcmStreamWithInfo(PcmStream, PcmStream.Format);
        }


        private int chan;

        public PcmFormatInfo Format { get; }
        private AudioFileDecodeStream(string url)
        {
            chan = BASS_StreamCreateFile(false, url, 0L, 0L, BASS_STREAM_DECODE | BASS_UNICODE);
            if (chan == 0)
                throw new Exception();

            BASS_CHANNELINFO info = new BASS_CHANNELINFO();
            BASS_ChannelGetInfo(chan, ref info);

            Format = new PcmFormatInfo(info.freq, System.Convert.ToByte(info.chans), (info.flags & BASS_SAMPLE_8BITS) != 0 ? PcmSampleFormat.U8 : PcmSampleFormat.S16);
        }


        public override bool CanWrite { get; } = false;
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
        public override void Flush()
        {
        }
        public override bool CanSeek { get; } = false;
        public override long Length
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }
        public override bool CanRead { get; } = true;
        private int _position = 0;
        private int buffer_bass_startPosition = 0;
        private byte[] buffer_bass = new byte[512];
        private int buffer_bass_length = 0;
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (chan == 0)
                return 0;
            int outputLength = 0;
            int numOfNeed;
            numOfNeed = count;
            while (numOfNeed > 0)
            {
                if (numOfNeed > 0)
                {
                    int buffer_bass_numOfUnread = buffer_bass_length - buffer_bass_startPosition;
                    if (buffer_bass_numOfUnread > 0)
                    {
                        if (buffer_bass_numOfUnread > numOfNeed)
                        {
                            System.Array.Copy(buffer_bass, buffer_bass_startPosition, buffer, offset + outputLength, numOfNeed);
                            _position += numOfNeed;
                            buffer_bass_startPosition += numOfNeed;

                            outputLength += numOfNeed;
                            numOfNeed = 0;
                        }
                        else
                        {
                            System.Array.Copy(buffer_bass, buffer_bass_startPosition, buffer, offset + outputLength, buffer_bass_numOfUnread);
                            _position += buffer_bass_numOfUnread;
                            buffer_bass_startPosition = 0;
                            buffer_bass_length = 0;

                            outputLength += buffer_bass_numOfUnread;
                            numOfNeed -= buffer_bass_numOfUnread;
                        }
                    }
                    else if (numOfNeed > buffer_bass.Length)
                    {
                        GCHandle p = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                        var numOfOutput = BASS_ChannelGetData(chan, (IntPtr)(p.AddrOfPinnedObject().ToInt64() + offset + outputLength), numOfNeed);
                        p.Free();
                        p = default(GCHandle);


                        if (numOfOutput > 0)
                        {
                            _position += numOfOutput;

                            outputLength += numOfOutput;
                            numOfNeed -= numOfOutput;
                        }
                        else
                            break;
                    }
                    else
                    {
                        buffer_bass_startPosition = 0;
                        buffer_bass_length = BASS_ChannelGetData(chan, buffer_bass, buffer_bass.Length);
                        if (buffer_bass_length < 0)
                        {
                            buffer_bass_length = 0;
                            break;
                        }
                    }
                }
            }
            return outputLength;
        }

        public override void Close()
        {
            base.Close();
            if (chan != 0)
            {
                BASS_StreamFree(chan);
                chan = 0;
            }
        }
    }
}
