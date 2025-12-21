# Engineering Service API

O **Engineering Service** integra a suíte **F1Simulator**, sendo responsável por **simular a evolução técnica dos carros**, **a progressão do handicap dos pilotos** e **o cálculo do Performance Delta (PD)** durante treinos, qualificação e corrida.

Este serviço atua como o **núcleo técnico** da temporada, refletindo o impacto direto do trabalho de engenharia e da experiência dos pilotos nos resultados esportivos.

---

## 🚀 Propósito do Módulo

Este microserviço representa o **Departamento de Engenharia** das equipes, com as seguintes responsabilidades:

- Evoluir os coeficientes técnicos dos carros (aerodinâmica e potência).
- Aplicar a influência direta dos engenheiros associados a cada carro.
- Atualizar o handicap dos pilotos após corridas completas.
- Calcular o **Performance Points (PD)** para:
  - Qualificação (grid fictício).
  - Corrida (resultado final).
- Persistir o estado evoluído de carros e pilotos em banco próprio.
- Disponibilizar endpoints de consulta para análises pós-evento.

---

## 📋 Diretrizes de Negócio

### Evolução dos Carros

- A evolução ocorre **em todos os treinos, qualificação e corrida**.
- Cada carro possui:
  - Um engenheiro aerodinâmico.
  - Um engenheiro de potência.
- **Se não houver engenheiro associado**, o coeficiente correspondente **não evolui**.

**Fórmula de evolução:**

```
C(novo) = C(atual) + (Experiência do Engenheiro × Fator Aleatório)
```

- Fator aleatório ∈ `[-1.000, +1.000]`
- Valores finais limitados entre `0.000` e `10.000`

---

### Evolução do Handicap do Piloto

- O handicap **só é atualizado após a corrida**.

```
H(novo) = H(atual) - (Experiência do Piloto × 0.5)
```

- O handicap nunca pode ser negativo.

---

### Performance Delta (PD)

```
PD = (Ca × 0.4) + (Cp × 0.4) - Handicap + Fator Sorte
```

- Calculado na **qualificação** e na **corrida**
- Fator Sorte ∈ `[-1.000, +1.000]`

---

## 🔗 Integração com Outros Serviços

### Team Management API

A Engineering API **não gerencia equipes, carros ou pilotos diretamente**.

Todos os dados base são obtidos via **HTTP Client** do serviço **Team Management**, incluindo:

- Carros ativos
- Pilotos associados
- Engenheiros e suas experiências

Após o processamento, os dados são **persistidos exclusivamente no banco da Engineering API**.

---

## 🛣️ Rotas da Aplicação

### 🔧 Gatilhos de Evolução

#### Treino Livre

Evolui apenas os carros do time informado.

```
POST /api/engineering/practice/{teamId}
```

---

#### Qualificação

- Evolui os carros
- Calcula o PD de qualificação

```
POST /api/engineering/qualifying/{teamId}
```

---

#### Corrida

- Evolui os carros
- Atualiza o handicap dos pilotos
- Calcula o PD da corrida

```
POST /api/engineering/race/{teamId}
```

---

## 📊 Endpoints de Consulta

### Listar Carros com Status Técnico

```
GET /api/engineering/cars
```

**Response**
```json
[
  {
    "carId": 1,
    "aerodynamicCoefficient": 7.325,
    "powerCoefficient": 6.980
  }
]
```

---

### Listar Handicaps dos Pilotos

```
GET /api/engineering/driver/handicaps
```

---

### Listar PD da Qualificação

```
GET /api/engineering/drivers/qualification
```

---

### Listar PD da Corrida

```
GET /api/engineering/pd
```

---

## 🗄️ Persistência de Dados

**Banco:** SQL Server — `EngineeringDB`

### Tabela `Cars`

```sql
CREATE TABLE Cars (
    Id INT PRIMARY KEY,
    AerodynamicCoefficient DECIMAL(5,3) NOT NULL,
    PowerCoefficient DECIMAL(5,3) NOT NULL
);
```

### Tabela `Drivers`

```sql
CREATE TABLE Drivers (
    Id INT PRIMARY KEY,
    Handicap DECIMAL(5,2) NOT NULL,
    QualifyingPd DECIMAL(6,3) NULL,
    RacePd DECIMAL(6,3) NULL
);
```

---

## ⚠️ Possíveis Erros

| Status Code | Motivo |
|------------|-------|
| 400 | Team inválido ou dados incompletos vindos do Team Management |
| 404 | Time não encontrado ou sem carros ativos |
| 500 | Falha de comunicação entre microserviços ou erro de persistência |

---

## 📌 Observações Finais

- Arquitetura baseada em **Microsserviços**
- Persistência isolada por contexto
- Implementação com **Dapper**
- Operações críticas assíncronas
- Serviço executável de forma independente, desde que o **Team Management** esteja disponível
