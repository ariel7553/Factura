using Facturacion.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Facturacion.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListaFacturas()
        {

            return View();
        }

        public string ListadoFacturas() {

            Resp resp = new Resp();
            using (FacturacionEntities context = new FacturacionEntities())
            {

                
                try
                {
                    var query = (from Fact in context.Factura
                                 
                                 join Cliente in context.Cliente on Fact.Id_Cliente equals Cliente.Cliente_Id

                                 select new FacturaModel
                                 {
                                     Num = 0,
                                     Id_Fact = Fact.Id_Fact,
                                     
                                     Cliente = Cliente.Nombre_Cliente + " " + Cliente.Apellido_Cliente,
                                     Fecha = Fact.Fecha,
                                     Total = Fact.Total,
                                     Iva = Fact.Iva,
                                     Subtotal = Fact.Subtotal,

                                 });


                    resp.Info = query.ToList();
                    return JsonConvert.SerializeObject(resp);
                }
                catch (Exception ex)
                {
                    resp.Mensaje = ex.Message;
                    return JsonConvert.SerializeObject(resp);
                }
            }
            
        }


        public ActionResult verFactura(int idFact) {
            Factura factura = new Factura();
            using (FacturacionEntities context = new FacturacionEntities())
            {
                factura = context.Factura.Include("Cliente").Include("Detalle").Where(x => x.Id_Fact == idFact).FirstOrDefault();
                if (factura != null)
                {
                    factura.Detalle = context.Detalle.Include("Producto").Where(x => x.Id_Fac == idFact).ToList();
                }
            }
            // ViewBag.Sub = f.Detalle_Factura.Sum(x => x.Cant * x.Precio).ToString("0.00");
            return View(factura);
        }

        public string GuardarFactura(Cliente cliente, List<ProductoModel> productos)
        {
            Resp resp = new Resp();
            using (FacturacionEntities context = new FacturacionEntities())
            {
                using (var scope = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        Factura factura = new Factura();
                        factura.Id_Cliente = cliente.Cliente_Id;

                        factura.Fecha = DateTime.Now;
                        decimal sub = productos.Sum(subtotal => subtotal.Cant * subtotal.Precio);
                        factura.Subtotal = sub;
                        factura.Iva = sub * Convert.ToDecimal(0.12);
                        factura.Total = sub + factura.Iva;
                        factura.Cliente = cliente;
                        factura.Detalle = new HashSet<Detalle>();
                        context.Factura.Add(factura);
                        context.SaveChanges();


                        foreach (var item in productos)
                        {
                            var productoBD = context.Producto.Where(x => x.Producto_Id == item.Producto_Id).FirstOrDefault();
                            if (productoBD != null)
                            {
                                if (productoBD.Stock >= item.Cant)
                                {
                                    productoBD.Stock = productoBD.Stock - item.Cant;

                                    context.SaveChanges();

                                    Detalle detalle = new Detalle();
                                    detalle.Id_Fac = factura.Id_Fact;
                                    detalle.Cantidad = item.Cant;
                                    detalle.Precio = productoBD.Precio;
                                    detalle.Id_Prod = productoBD.Producto_Id;
                                    detalle.Total = productoBD.Precio * item.Cant;
                                    context.Detalle.Add(detalle);
                                    context.SaveChanges();

                                }
                                else
                                {
                                    scope.Rollback();

                                    resp.Mensaje = "Cantidad Insuficiente, Producto: " + item.Producto1 + ", Stock: " + productoBD.Stock;
                                    return JsonConvert.SerializeObject(resp);
                                }
                            }
                            else
                            {
                                scope.Rollback();

                                resp.Mensaje = "No existe el producto: " + item.Producto1;
                                return JsonConvert.SerializeObject(resp);
                            }
                        }

                        scope.Commit();

                        return JsonConvert.SerializeObject(resp);
                    }

                    catch (DbEntityValidationException e)
                    {

                        scope.Rollback();
                        resp.Mensaje = e.Message;
                        return JsonConvert.SerializeObject(resp);
                    }

                    catch (Exception ex)
                    {
                        scope.Rollback();

                        resp.Mensaje = ex.Message;
                        return JsonConvert.SerializeObject(resp);
                    }

                }
            }


        }



        public string Clientes()
        {
            Resp resp = new Resp();
            try
            {
                FacturacionEntities facturacionEntities = new FacturacionEntities();
                resp.Info = facturacionEntities.Cliente.Where(x => x.Estado == 1).ToList();
            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.Message;
            }

            return JsonConvert.SerializeObject(resp);
        }

        public string Productos()
        {
            Resp resp = new Resp();
            try
            {
                FacturacionEntities facturacionEntities = new FacturacionEntities();

                List<ProductoModel> productos = new List<ProductoModel>();
                foreach (var item in facturacionEntities.Producto.Where(x => x.Estado == 1).ToList())
                {
                    ProductoModel p = new ProductoModel();
                    p.Producto_Id = item.Producto_Id;
                    p.Stock = item.Stock;
                    p.Precio = item.Precio;
                    p.Producto1 = item.Producto1;
                    p.Estado = item.Estado;
                    productos.Add(p);
                }

                resp.Info = productos;

            }
            catch (Exception ex)
            {
                resp.Mensaje = ex.Message;
            }

            return JsonConvert.SerializeObject(resp);
        }
    }
}