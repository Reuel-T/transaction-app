<template>
  <div>
    <h3>Other Info Goes here</h3>
  </div>
  <div class="scroll">
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

<style scoped>
  .scroll {
    height: 100%;
    overflow-y: auto;
  }
</style>
