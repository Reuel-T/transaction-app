<template>
  <div class="title-row">
    <h2>Transactions</h2>
    <label for="sortField">Sort:</label>
    <select
      id="sortField"
      v-model="sortField"
    >
      <option value="transactionID">Chronological</option>
      <option value="amount">Amount</option>
    </select>
    <select v-model="sortOrder">
      <option value="asc">ASC</option>
      <option value="desc">DESC</option>
    </select>
  </div>

  <div class="heading-row">
    <div class="heading-row-item">
      <h3>Amount</h3>
    </div>
    <div class="heading-row-item">
      <h3>Transaction Type</h3>
    </div>
    <div class="heading-row-item">
      <h3>Comment</h3>
    </div>
    <div class="heading-row-end"></div>
  </div>

  <div class="scroll">
    <ClientTransaction
      v-for="transaction in sortedTransactions"
      :key="transaction.transactionID"
      :transaction="transaction"
    />
  </div>
</template>

<script setup lang="ts">
  import ClientTransaction from './ClientTransaction.vue'
  import type { ClientTransactionDTO } from '@/models/ClientTransactionDTO'
  import type { SortOrder } from '@/types/SortOrder'
  import type { TransactionSortType } from '@/types/TransactionSortTypes'
  import { ref, type PropType, computed } from 'vue'

  const props = defineProps({
    transactions: {
      required: true,
      type: Array as PropType<ClientTransactionDTO[]>
    }
  })

  const sortField = ref<TransactionSortType>('transactionID')
  const sortOrder = ref<SortOrder>('asc')

  const sortedTransactions = computed(() => {
    if (sortOrder.value === 'asc') {
      return [...props.transactions].sort((t1: ClientTransactionDTO, t2: ClientTransactionDTO) => {
        return t1[sortField.value] > t2[sortField.value] ? 1 : -1
      })
    } else {
      return [...props.transactions].sort((t1: ClientTransactionDTO, t2: ClientTransactionDTO) => {
        return t1[sortField.value] < t2[sortField.value] ? 1 : -1
      })
    }
  })
</script>

<style lang="scss" scoped>
  .scroll {
    height: 100%;
    overflow-y: auto;

    scrollbar-width: thin;
  }

  .title-row {
    align-items: center;
    display: flex;
    flex-direction: row;
    h2 {
      margin: 0;
      flex-grow: 1;
    }
    padding: 0 1rem;
  }

  .heading-row {
    display: flex;
    flex-direction: row;

    .heading-row-item {
      width: 30%;
      h3 {
        text-align: center;
      }
    }

    .heading-row-end {
      width: 10%;
    }
  }
</style>
