[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/samuel-lucas6/Rumba.NET/blob/main/LICENSE)
# Rumba.NET
A .NET implementation of [Rumba20, Rumba12, and Rumba8](https://cr.yp.to/rumba20.html). [Salsa20](https://cr.yp.to/snuffle/salsafamily-20071225.pdf) can be replaced with [ChaCha](https://cr.yp.to/chacha/chacha-20080128.pdf) or [Forró](https://link.springer.com/article/10.1007/s00145-023-09455-5), just [make sure](https://cr.yp.to/rumba20/newfeatures-20071218.pdf) the feedforward additions are present.

> **Warning**
>
> Rumba20 is _not_ designed to provide unpredictability, truncated collision resistance, etc. These features must be provided by an appropriate output filter (e.g. a cryptographic hash function). Rumba20's goal is to efficiently compress a long input so that only a small amount of data has to be handled by the output filter.
