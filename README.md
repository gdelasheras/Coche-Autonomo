# Coche autónomo mediante un percentrón multicapa y algoritmos genéticos

## Introducción
### Objetivo
Este proyecto tiene como objetivo aunar todos los conocimientos adquiridos acerca de la inteligencia artificial. Para ello, se ha propuesto el desarrollo de un vehículo autónomo que aprenda a recorrer un trazado propuesto, sin especificar las instrucciones concretas para recorrerlo. El vehículo, funcionará gracias a la combinación de un MLP y de un algoritmo genético.
El MLP contendrá las distintas soluciones al problema y el algoritmo genético proporcionará el marco de evolución para encontrar las mejores soluciones.
### Alcance
Debido a la naturaleza de la asignatura, el objetivo primordial es el correcto funcionamiento de
la inteligencia artificial que hace que el vehículo funcione. Todos los extras visuales se
consideran en un segundo plano.
Para el vehículo y para el mapa, se han descargado assets del mercado oficial de Unity, Unity
Market.
Debido al tipo de trazado obtenido, con todo tipo de curvas rápidas, lentas, cerradas y abiertas,
el entrenamiento requerido es muy amplio, por lo que se considerará éxito que el vehículo
recorra correctamente 1 vuelta completa (o más) al trazado.
### Herramientas utilizadas
Para el desarrollo de este proyecto se han empleado las siguientes herramientas software:

<ul>
  <li>Motor 3D Unity3D.</li>
  <li>Visual Studio 2017.</li>
</ul>

Todo el desarrollo se ha realizado con el lenguaje C#.

## Mapa
El trazado propuesto para el vehículo autónomo se ha elegido atendiendo al diseño del
vehículo (sensores de distancia) de modo que este sea adecuado para su modo de funcionar.
De esta forma, el mapa está ambientado en un puerto lleno de contenedores los cuales cubren
en trazado en su totalidad para permitir a los sensores medir las distancias correctamente.

<p align="center">
  <img src="https://github.com/gonzzard/Coche-Autonomo/blob/master/IMG/Captura%20copia.png" align="center" height="400" width="400">
  <img src="https://github.com/gonzzard/Coche-Autonomo/blob/master/IMG/Captura.PNG" align="center" height="400" width="400">
</p>

El recorrido comienza en el punto marcada con una M (Meta) y continua según el orden
marcado en la imagen anterior.

## Vehículo
El diseño del vehículo consta de 7 sensores de distancias (máximo 50 unidades) posicionados
en la parte frontal cada uno con un grado de inclinación como marca la imagen siguiente.

<p align="center">
  <img src="https://github.com/gonzzard/Coche-Autonomo/blob/master/IMG/cochedesig%20copia.png" align="center" height="400" width="400">
</p>

El diseño en abanico está orientado a cubrir el mayor espacio posible para detectar obstáculos
y reducir los puntos ciegos sin que suponga un número alto de sensores, pues al tratarse de
las entradas proporcionadas a la red neuronal, a más sensores, más entradas y por tanto más
pesos y cálculos a realizar.

<p align="center">
  <img src="https://github.com/gonzzard/Coche-Autonomo/blob/master/IMG/Captura1.PNG" align="center" height="400" width="400">
</p>

Las especificaciones técnicas del vehículo son:

<ul>
  <li>Para los giros de izquierda y derecha se ha establecido 30º como ángulo máximo de giro.</li>
  <li>El número máximo de aceleración está establecido en 900 unidades.</li>
  <li>Para girar, al objeto de Unity se le proporciona un valor entre -1 y 1 el cual se multiplica por el valor máximo de giro (30º) resultando:
    <ul>
      <li>Negativo (< 0º): Giro a la izquierda tantos grados como indique el valor resultante.</li>
      <li>Positivo (> 0º): Giro a la derecha tantos grados como indique el valor resultante.</li>
    </ul>
  </li>
  <li>Para acelerar/frenar, al objeto de Unity se le proporciona un valor entre -1 y 1.</li>
  <li>Las físicas del vehículo son las normales proporcionadas por el motor gráfico Unity</li>
</ul>

## MLP
Tras un proceso de experimentación para encontrar la mejor arquitectura para la red neuronal,
ésta se ha diseñado de la siguiente forma:

<ul>
  <li>Capa de entrada (7 neuronas):
    <ul>
      <li>Se corresponden a las 7 distancias medidas por los sensores del vehículo normalizadas.</li>
    </ul>
  </li>
  <li>Capa oculta (5 neuronas):
    <ul>
      <li>Función de activación: tangente hiperbólica.</li>
    </ul>
  </li>
  <li>Capa de salida (2 neuronas):
    <ul>
      <li>1 neurona representa el par motor aplicado siendo:
          <ul>
            <li>Positivo: Adelante (acelerar).</li>
            <li>Negativo: Hacia atrás (frenar/marcha atrás)</li>
          </ul>
      </li>
      <li>1 neurona representa el ángulo de giro del volante (y por tanto de las ruedas):
          <ul>
            <li>Negativo: Izquierda.</li>
            <li>Positivo: Derecha.</li>
          </ul>
      </li>
      <li>Función de activación: tangente hiperbólica.</li>
    </ul>
  </li>
 </ul>
  
## Algoritmo genético

La selección de los individuos padres se realiza mediante la selección del 20% de la población
con mayor fitness, 4 individuos, los cuales generan la siguiente población mediante
replicaciones con posibles mutaciones (selección elitista).

La mutación de los genes se realiza mediante un método de Montecarlo. Se selecciona un
número aleatorio entre 0 y 1 el cual se compara con el factor de calidad (Q) elegido. Si la tirada
es mayor que Q, se produce una mutación resultando el gen un nuevo número aleatorio. En
caso de que la tirada sea menor que Q, el gen se copia exactamente igual.

## Capturas

<p align="center">
  <img src="https://github.com/gonzzard/Coche-Autonomo/blob/master/IMG/2325f13c47495cb163b343ca94e3ebe2.png">
</p>

<p align="center">
  <img src="https://github.com/gonzzard/Coche-Autonomo/blob/master/IMG/669e7acad45efc7ca68ebf5a729a1a18.jpg">
</p>

<p align="center">
  <img src="https://github.com/gonzzard/Coche-Autonomo/blob/master/IMG/68f6a574f7c6e8b5e5afebe91da681f8.jpg">
</p>

<p align="center">
  <img src="https://github.com/gonzzard/Coche-Autonomo/blob/master/IMG/c7551837403dc61946ab904650490828.jpg">
</p>

## Vídeo
[![Vídeo coche autónomo](https://github.com/gonzzard/Coche-Autonomo/blob/master/IMG/2325f13c47495cb163b343ca94e3ebe2.png)](https://www.youtube.com/embed/-JP8tjK6XmI "Vídeo coche autónomo")
