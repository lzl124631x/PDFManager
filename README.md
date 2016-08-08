PDFManager
==========

A tool for managing the meta data of PDF files.

---

This is a tool I implemented in the first semester of my postgraduate. I learned a lot about the PDF file structure (the xref, objects, etc.) and implemented firstly a C# version and then a pure C version.

Today I cloned this repo and built it. Sadly it only works for a small portion of PDFs (a large portion of PDFs are linearized, which this tool doesn't support). I had the impulse to rewrite this tool in JS but found there was already well implemented [Mozilla/pdf.js](https://github.com/mozilla/pdf.js), so I gave up. I need to save my time doing more meaningful things.

Good day,
Richard
2016-08-08