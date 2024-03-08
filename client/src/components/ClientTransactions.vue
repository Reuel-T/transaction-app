<template>
  <div>
    <h1>Transactions</h1>
    <h3 v-if="transactions.length < 1">No Transactions for this client</h3>
    <ul v-else>
      <li v-for="transaction in transactions" :key="transaction.TransactionID">
        {{ transaction }}
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
  import type { ClientTransaction } from '@/models/ClientTransaction'
  import type { PropType } from 'vue'

  defineProps({
    transactions: {
      required: true,
      type: Array as PropType<ClientTransaction[]>
    }
  })
</script>

<style scoped></style>
