Dead Letter Queue
Introdução: Uma Dead Letter Queue (DLQ), ou "Fila de Letras Mortas", é um conceito
usado em sistemas de mensageria (como RabbitMQ, AWS SQS, Apache Kafka, etc.) para
lidar com mensagens que não puderam ser processadas corretamente.
 Conceito Básico
Quando uma mensagem não pode ser consumida ou processada por um consumidor por
algum motivo (por exemplo, erro no formato, falha de validação, timeout, etc.), ela é
redirecionada para a Dead Letter Queue.
 Finalidade
A DLQ serve para:
• Evitar a perda de mensagens.
• Isolar mensagens problemáticas, evitando que travem o fluxo principal.
• Permitir análise posterior para identificar e corrigir problemas.
• Ajudar no reprocessamento após correção do erro.
 Quando uma mensagem vai para a DLQ?
Depende da configuração, mas alguns motivos comuns são:
• Número máximo de tentativas de processamento excedido.
• Erros de lógica no consumidor.
• Mensagens com TTL expirado.
Escolha do Exchange: RabbitMQ
O RabbitMQ foi escolhido por oferecer a mensagem é entregue a apenas a um
consumidor que esta inscrito na queue garantindo que não haja duplicidade de uma
mensagem.
Direct Exchange
Roteamento baseado em chave (routing key).
• A mensagem vai para a(s) fila(s) cuja binding key corresponde exatamente à
routing key da mensagem.
• Uso típico: Entregar mensagens a filas específicas.
Exemplo:
Mensagem com routing key = pedido.novo vai para fila com binding key = pedido.novo.
Configuração das Filas:
fila.pedidos ----- Routing Key (pedidos) ----- Subscriber (Processar Pedido)
Dead Letter Queues (DLQ):
Cada fila principal terá uma DLQ configurada para mensagens com falha:
• fila.criar.pedido.dlq / Routing Key (DLQ)
Subscribers e Consumo:
-Comportamento dos Subscribers
Cada Subscriber ficará escutando sua respectiva fila. Em caso de falha, a mensagem não
vai ser confirmada (nack)
• Após a falha, a mensagem vai para a DLQ.
Planejamento das APIs REST:
APIs Consumidas pelos Subscribers
API Método Rota Descrição
Criar Pedido POST /api/pedidos Cria novo pedido
Formato dos Dados (JSON): Exemplo: Criar Pedido
{
 "nomeCliente": "jose",
 "nomeProduto": "chiclete",
 "valor": 1.45,
“quantidade”: 6,
}
Cronograma:
- 12/05/2025: Diagrama do Sistema / Planejamento de Apis / Plano de Trabalho
- 17/05/2025: Desenvolvimento
- 19/05/2025:Codigo Publisher / Codigo Subiscriber / Teste de Comunicação
- 26/05/2025:Projeto funcionando / GitHub do Projeto / PPT para apresentar /
Tarefas de Membros do Grupo:
-Leandro de Oliveira Poletti RA-1982910: Desenvolvimento das APIs REST /
Implementação dos Publishers
-Caio Henrique Ocon RA-1958689: Diagramação / Setup do RabbitMQ, criação das
filas e DQL
-Breno Henrique Colonello RA-1958402: Documentação técnica / Implementação
dos Subscribers
