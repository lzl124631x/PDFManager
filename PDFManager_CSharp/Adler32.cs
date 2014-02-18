using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDFManager_CSharp
{
    class Adler32
    {
        const int BASE = 65521; /* largest prime smaller than 65536 */
        /*
        Update a running Adler-32 checksum with the bytes buf[0..len-1]
        and return the updated checksum. The Adler-32 checksum should be
        initialized to 1.
        Usage example:
        ulong adler = 1L;
        while (read_buffer(buffer, length) != EOF) {
        adler = update_adler32(adler, buffer, length);
        }
        if (adler != original_adler) error();
        */
        ulong update_adler32(ulong adler, char[] buf, int len)
        {
            ulong s1 = adler & 0xffff;
            ulong s2 = (adler >> 16) & 0xffff;
            int n;
            for (n = 0; n < len; n++)
            {
                s1 = (s1 + buf[n]) % BASE;
                s2 = (s2 + s1) % BASE;
            }
            return (s2 << 16) + s1;
        }
        /* Return the adler32 of the bytes buf[0..len-1] */
        ulong adler32(char[] buf, int len)
        {
            return update_adler32(1L, buf, len);
        }
    }
}
