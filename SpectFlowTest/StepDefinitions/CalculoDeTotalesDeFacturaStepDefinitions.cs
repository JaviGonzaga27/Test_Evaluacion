using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using FluentAssertions;
using EvaluacionGonzagaJavier.Models;
using TechTalk.SpecFlow.Assist;

namespace SpectFlowTest.StepDefinitions
{
    [Binding]
    public class CalculoTotalesStepDefinitions
    {
        private List<Order> _orders;
        private decimal _discountPercentage;
        private decimal _subtotal;
        private decimal _discount;
        private decimal _total;

        [Given(@"Tengo los siguientes productos en mi factura")]
        public void GivenTengoLosSiguientesProductosEnMiFactura(Table table)
        {
            _orders = table.CreateSet<Order>().ToList();
        }

        [Given(@"Aplico un descuento del (.*)%")]
        public void GivenAplicoUnDescuentoDel(decimal discountPercentage)
        {
            _discountPercentage = discountPercentage;
        }

        [When(@"Calculo el total de la factura sin descuento")]
        public void WhenCalculoElTotalDeLaFacturaSinDescuento()
        {
            CalculateTotals(0);
        }

        [When(@"Calculo el total de la factura")]
        public void WhenCalculoElTotalDeLaFactura()
        {
            CalculateTotals(_discountPercentage);
        }

        [Then(@"El subtotal debe ser (.*)")]
        public void ThenElSubtotalDebeSer(decimal expectedSubtotal)
        {
            _subtotal.Should().Be(expectedSubtotal);
        }

        [Then(@"El descuento debe ser (.*)")]
        public void ThenElDescuentoDebeSer(decimal expectedDiscount)
        {
            _discount.Should().Be(expectedDiscount);
        }

        [Then(@"El total debe ser (.*)")]
        public void ThenElTotalDebeSer(decimal expectedTotal)
        {
            _total.Should().Be(expectedTotal);
        }

        private void CalculateTotals(decimal discountPercentage)
        {
            _subtotal = _orders.Sum(o => o.UnitPrice * o.Quantity);
            _discount = _subtotal * (discountPercentage / 100);
            _total = _subtotal - _discount;
        }
    }
}