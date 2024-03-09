<template>
  
    <h3 v-if="transactions.length < 1">No Transactions for this client</h3>
    <div v-else>
      <ClientTransaction
        v-for="transaction in transactions"
        :key="transaction.transactionID"
        :transaction="transaction"
      />
    </div>
  
</template>

<script setup lang="ts">
  import ClientTransaction from './ClientTransaction.vue'
  import type { ClientTransactionDTO } from '@/models/ClientTransactionDTO'
  import type { PropType } from 'vue'

  defineProps({
    transactions: {
      required: true,
      type: Array as PropType<ClientTransactionDTO[]>
    }
  })
</script>

<style scoped></style>
@/models/ClientTransactionDTO
