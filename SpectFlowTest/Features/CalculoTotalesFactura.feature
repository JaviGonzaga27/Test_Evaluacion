Feature: Cálculo de Totales de Factura

Scenario: Calcular Total sin Descuento
  Given Tengo los siguientes productos en mi factura
    | ProductName | UnitPrice | Quantity |
    | Producto A  | 100.00    | 2        |
  When Calculo el total de la factura sin descuento
  Then El subtotal debe ser 200.00
  And El descuento debe ser 0.00
  And El total debe ser 200.00

Scenario: Calcular Total con Descuento
  Given Tengo los siguientes productos en mi factura
    | ProductName | UnitPrice | Quantity |
    | Producto B  | 100.00     | 2        |
  And Aplico un descuento del 50%
  When Calculo el total de la factura
  Then El subtotal debe ser 200.00
  And El descuento debe ser 100.00
  And El total debe ser 100.00

Scenario: Calcular Total con Múltiples Productos
  Given Tengo los siguientes productos en mi factura
    | ProductName | UnitPrice | Quantity |
    | Producto C  | 60.00     | 2        |
    | Producto D  | 45.00     | 1        |
    | Producto E  | 50.00     | 4        |
  When Calculo el total de la factura sin descuento
  Then El subtotal debe ser 365.00
  And El descuento debe ser 0.00
  And El total debe ser 365.00

Scenario: Calcular Total con Cantidades Grandes
  Given Tengo los siguientes productos en mi factura
    | ProductName | UnitPrice | Quantity |
    | Producto F  | 4.99      | 10000    |
    | Producto G  | 9.00      | 20000    |
  When Calculo el total de la factura sin descuento
  Then El subtotal debe ser 229900.00
  And El descuento debe ser 0.00
  And El total debe ser 229900.00