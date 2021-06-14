using System.Collections.Generic;
using System.IO;

namespace sievis.Common
{
    public static class MimeTypeProvider
    {
        public const string DefaultExt = "asc";
        public const string NewLine = "\n";
        public const string Tab = "\t";

        public static readonly Dictionary<string, string> MTypes = new Dictionary<string, string>();
        static MimeTypeProvider()
        {
            MTypes.Add("ai", "application/postscript");
            MTypes.Add("aif", "audio/x-aiff");
            MTypes.Add("aifc", "audio/x-aiff");
            MTypes.Add("aiff", "audio/x-aiff");
            MTypes.Add("asc", "text/plain");
            MTypes.Add("atom", "application/atom+xml");
            MTypes.Add("au", "audio/basic");
            MTypes.Add("avi", "video/x-msvideo");
            MTypes.Add("bcpio", "application/x-bcpio");
            MTypes.Add("bin", "application/octet-stream");
            MTypes.Add("bmp", "image/bmp");
            MTypes.Add("cdf", "application/x-netcdf");
            MTypes.Add("cgm", "image/cgm");
            MTypes.Add("class", "application/octet-stream");
            MTypes.Add("cpio", "application/x-cpio");
            MTypes.Add("cpt", "application/mac-compactpro");
            MTypes.Add("csh", "application/x-csh");
            MTypes.Add("css", "text/css");
            MTypes.Add("dcr", "application/x-director");
            MTypes.Add("dif", "video/x-dv");
            MTypes.Add("dir", "application/x-director");
            MTypes.Add("djv", "image/vnd.djvu");
            MTypes.Add("djvu", "image/vnd.djvu");
            MTypes.Add("dll", "application/octet-stream");
            MTypes.Add("dmg", "application/octet-stream");
            MTypes.Add("dms", "application/octet-stream");
            MTypes.Add("doc", "application/msword");
            MTypes.Add("dtd", "application/xml-dtd");
            MTypes.Add("dv", "video/x-dv");
            MTypes.Add("dvi", "application/x-dvi");
            MTypes.Add("dxr", "application/x-director");
            MTypes.Add("eps", "application/postscript");
            MTypes.Add("etx", "text/x-setext");
            MTypes.Add("exe", "application/octet-stream");
            MTypes.Add("ez", "application/andrew-inset");
            MTypes.Add("gif", "image/gif");
            MTypes.Add("gram", "application/srgs");
            MTypes.Add("grxml", "application/srgs+xml");
            MTypes.Add("gtar", "application/x-gtar");
            MTypes.Add("hdf", "application/x-hdf");
            MTypes.Add("hqx", "application/mac-binhex40");
            MTypes.Add("htm", "text/html");
            MTypes.Add("html", "text/html");
            MTypes.Add("ice", "x-conference/x-cooltalk");
            MTypes.Add("ico", "image/x-icon");
            MTypes.Add("ics", "text/calendar");
            MTypes.Add("ief", "image/ief");
            MTypes.Add("ifb", "text/calendar");
            MTypes.Add("iges", "model/iges");
            MTypes.Add("igs", "model/iges");
            MTypes.Add("jnlp", "application/x-java-jnlp-file");
            MTypes.Add("jp2", "image/jp2");
            MTypes.Add("jpe", "image/jpeg");
            MTypes.Add("jpeg", "image/jpeg");
            MTypes.Add("jpg", "image/jpeg");
            MTypes.Add("js", "application/x-javascript");
            MTypes.Add("kar", "audio/midi");
            MTypes.Add("latex", "application/x-latex");
            MTypes.Add("lha", "application/octet-stream");
            MTypes.Add("lzh", "application/octet-stream");
            MTypes.Add("m3u", "audio/x-mpegurl");
            MTypes.Add("m4a", "audio/mp4a-latm");
            MTypes.Add("m4b", "audio/mp4a-latm");
            MTypes.Add("m4p", "audio/mp4a-latm");
            MTypes.Add("m4u", "video/vnd.mpegurl");
            MTypes.Add("m4v", "video/x-m4v");
            MTypes.Add("mac", "image/x-macpaint");
            MTypes.Add("man", "application/x-troff-man");
            MTypes.Add("mathml", "application/mathml+xml");
            MTypes.Add("me", "application/x-troff-me");
            MTypes.Add("mesh", "model/mesh");
            MTypes.Add("mid", "audio/midi");
            MTypes.Add("midi", "audio/midi");
            MTypes.Add("mif", "application/vnd.mif");
            MTypes.Add("mov", "video/quicktime");
            MTypes.Add("movie", "video/x-sgi-movie");
            MTypes.Add("mp2", "audio/mpeg");
            MTypes.Add("mp3", "audio/mpeg");
            MTypes.Add("mp4", "video/mp4");
            MTypes.Add("mpe", "video/mpeg");
            MTypes.Add("mpeg", "video/mpeg");
            MTypes.Add("mpg", "video/mpeg");
            MTypes.Add("mpga", "audio/mpeg");
            MTypes.Add("ms", "application/x-troff-ms");
            MTypes.Add("msh", "model/mesh");
            MTypes.Add("mxu", "video/vnd.mpegurl");
            MTypes.Add("nc", "application/x-netcdf");
            MTypes.Add("oda", "application/oda");
            MTypes.Add("ogg", "application/ogg");
            MTypes.Add("pbm", "image/x-portable-bitmap");
            MTypes.Add("pct", "image/pict");
            MTypes.Add("pdb", "chemical/x-pdb");
            MTypes.Add("pdf", "application/pdf");
            MTypes.Add("pgm", "image/x-portable-graymap");
            MTypes.Add("pgn", "application/x-chess-pgn");
            MTypes.Add("pic", "image/pict");
            MTypes.Add("pict", "image/pict");
            MTypes.Add("png", "image/png");
            MTypes.Add("pnm", "image/x-portable-anymap");
            MTypes.Add("pnt", "image/x-macpaint");
            MTypes.Add("pntg", "image/x-macpaint");
            MTypes.Add("ppm", "image/x-portable-pixmap");
            MTypes.Add("ppt", "application/vnd.ms-powerpoint");
            MTypes.Add("ps", "application/postscript");
            MTypes.Add("qt", "video/quicktime");
            MTypes.Add("qti", "image/x-quicktime");
            MTypes.Add("qtif", "image/x-quicktime");
            MTypes.Add("ra", "audio/x-pn-realaudio");
            MTypes.Add("ram", "audio/x-pn-realaudio");
            MTypes.Add("ras", "image/x-cmu-raster");
            MTypes.Add("rdf", "application/rdf+xml");
            MTypes.Add("rgb", "image/x-rgb");
            MTypes.Add("rm", "application/vnd.rn-realmedia");
            MTypes.Add("roff", "application/x-troff");
            MTypes.Add("rtf", "text/rtf");
            MTypes.Add("rtx", "text/richtext");
            MTypes.Add("sgm", "text/sgml");
            MTypes.Add("sgml", "text/sgml");
            MTypes.Add("sh", "application/x-sh");
            MTypes.Add("shar", "application/x-shar");
            MTypes.Add("silo", "model/mesh");
            MTypes.Add("sit", "application/x-stuffit");
            MTypes.Add("skd", "application/x-koan");
            MTypes.Add("skm", "application/x-koan");
            MTypes.Add("skp", "application/x-koan");
            MTypes.Add("skt", "application/x-koan");
            MTypes.Add("smi", "application/smil");
            MTypes.Add("smil", "application/smil");
            MTypes.Add("snd", "audio/basic");
            MTypes.Add("so", "application/octet-stream");
            MTypes.Add("spl", "application/x-futuresplash");
            MTypes.Add("src", "application/x-wais-source");
            MTypes.Add("sv4cpio", "application/x-sv4cpio");
            MTypes.Add("sv4crc", "application/x-sv4crc");
            MTypes.Add("svg", "image/svg+xml");
            MTypes.Add("swf", "application/x-shockwave-flash");
            MTypes.Add("t", "application/x-troff");
            MTypes.Add("tar", "application/x-tar");
            MTypes.Add("tcl", "application/x-tcl");
            MTypes.Add("tex", "application/x-tex");
            MTypes.Add("texi", "application/x-texinfo");
            MTypes.Add("texinfo", "application/x-texinfo");
            MTypes.Add("tif", "image/tiff");
            MTypes.Add("tiff", "image/tiff");
            MTypes.Add("tr", "application/x-troff");
            MTypes.Add("tsv", "text/tab-separated-values");
            MTypes.Add("txt", "text/plain");
            MTypes.Add("ustar", "application/x-ustar");
            MTypes.Add("vcd", "application/x-cdlink");
            MTypes.Add("vrml", "model/vrml");
            MTypes.Add("vxml", "application/voicexml+xml");
            MTypes.Add("wav", "audio/x-wav");
            MTypes.Add("wbmp", "image/vnd.wap.wbmp");
            MTypes.Add("wbmxl", "application/vnd.wap.wbxml");
            MTypes.Add("wml", "text/vnd.wap.wml");
            MTypes.Add("wmlc", "application/vnd.wap.wmlc");
            MTypes.Add("wmls", "text/vnd.wap.wmlscript");
            MTypes.Add("wmlsc", "application/vnd.wap.wmlscriptc");
            MTypes.Add("wrl", "model/vrml");
            MTypes.Add("xbm", "image/x-xbitmap");
            MTypes.Add("xht", "application/xhtml+xml");
            MTypes.Add("xhtml", "application/xhtml+xml");
            MTypes.Add("xls", "application/vnd.ms-excel");
            MTypes.Add("xml", "application/xml");
            MTypes.Add("xpm", "image/x-xpixmap");
            MTypes.Add("xsl", "application/xml");
            MTypes.Add("xslt", "application/xslt+xml");
            MTypes.Add("xul", "application/vnd.mozilla.xul+xml");
            MTypes.Add("xwd", "image/x-xwindowdump");
            MTypes.Add("xyz", "chemical/x-xyz");
            MTypes.Add("zip", "application/zip");
        }

        public static bool ContainsExt(string ext)
        {
            return MTypes.ContainsKey(ext);
        }

        public static string GetContent(string ext)
        {
            return MTypes.ContainsKey(ext) ? MTypes[ext] : MTypes[DefaultExt];
        }

        public static string GetExtension(this string fileName)
        {
            return !string.IsNullOrEmpty(fileName) && Path.HasExtension(fileName)
                ? Path.GetExtension(fileName).Substring(1) : string.Empty;
        }
    }
}
    
