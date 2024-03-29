﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MoteeQueso.B2C.Product.Api.ViewModels;
using MoteeQueso.B2C.Product.Core.Interfaces;
using MoteeQueso.B2C.Product.Core.Services;
using MoteeQueso.B2C.Product.Infraestructure.Entities;

namespace MoteeQueso.B2C.Product.Api.Controllers
{
    [EnableCors("AllowMyOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController()
        {
            productService = new ProductService();
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(int? id, string query, int page = 1, int count = 5)
        {
            if (id.HasValue)
            {
                producto producto = await productService.GetProductById(id.Value);

                if (producto != null)
                {
                    ProductDetailViewModel productDetailViewModel = new ProductDetailViewModel
                    {
                        id = producto.id,
                        codigo = producto.codigo,
                        nombre = producto.nombre,
                        descripcion = producto.descripcion,
                        url_imagen = producto.url_imagen,
                        fecha_espectaculo = producto.fecha_espectaculo,
                        fecha_llegada = producto.fecha_llegada,
                        fecha_salida = producto.fecha_salida,
                        ciudad = producto.ciudad.nombre,
                        precio = (producto.ciudad.tarifa_ciudad.precio
                        + producto.tarifa_espectaculo.precio
                        + producto.tarifa_hospedaje.precio
                        + producto.tarifa_transporte.precio)
                    };

                    return Ok(productDetailViewModel);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                List<producto> productos = await productService.GetProductsByQuery(query, page, count);

                if (productos.Count > 0)
                {
                    List<ProductViewModel> productViewModels = new List<ProductViewModel>();

                    foreach (producto producto in productos)
                    {
                        productViewModels.Add(new ProductViewModel
                        {
                            id = producto.id,
                            codigo = producto.codigo,
                            nombre = producto.nombre,
                            url_imagen = producto.url_imagen
                        });
                    }

                    return Ok(productViewModels);
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}