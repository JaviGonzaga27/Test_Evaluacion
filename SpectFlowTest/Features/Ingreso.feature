Feature: Ingreso

Scenario: IngresoDeLaOrden
  Given Seleccionar los Datos para ingresar en la BDD
    | ProductName | UnitPrice | Quantity | Discount |
    | Product1    | 100       | 2        | 50.00    |
  When Calculamos los totales
  Then Los totales calculados son correctos
    | Subtotal | DiscountAmount | Total  |
    | 200.00   | 100.00         | 100.00 |
  When Ingresamos el registro a la BDD
  Then Resultado del registro ingresado a la BDD
    | ProductName | UnitPrice | Quantity | Discount | Subtotal | DiscountAmount | Total  |
    | Product1    | 100       | 2        | 50.00    | 200.00   | 100.00         | 100.00 |

 Scenario: EdicionDeLaOrden
  Given Existe una orden en la BDD
    | ProductName | UnitPrice | Quantity | Discount |
    | Disco Duro    | 125       | 8        | 50.00    |
  When Editamos la orden
    | ProductName | UnitPrice | Quantity | Discount |
    | Disco Duro    | 125       | 8        | 50.00    |
  Then La orden editada se refleja en la BDD
    | ProductName | UnitPrice | Quantity | Discount | Subtotal | DiscountAmount | Total |
    | Disco Duro  | 125       | 8        | 50.00    | 1000.00  | 500            | 500   |

Scenario: EliminacionDeLaOrden
  Given Existe una orden en la BDD
    | ProductName | UnitPrice | Quantity | Discount |
    | Product2    | 200       | 1        | 10.00    |
  When Eliminamos la orden
  Then La orden no existe en la BDD

Scenario: LecturaDeLaOrden
  Given Existe una orden en la BDD
    | ProductName | UnitPrice | Quantity | Discount |
    | Product3    | 75        | 4        | 15.00    |
  When Leemos la orden
  Then Los detalles de la orden son correctos
    | ProductName | UnitPrice | Quantity | Discount | Subtotal | DiscountAmount | Total  |
    | Product3    | 75        | 4        | 15.00    | 300.00   | 45.00          | 255.00 |