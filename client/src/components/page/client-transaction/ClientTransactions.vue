<template>
  <div class="title-row">
    <h2>Transactions</h2>
    <select v-model="sortField">
      <option value="transactionID">Chronological</option>
      <option value="amount">Amount</option>
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
    <div class="heading-row-end">
      
    </div>
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
  import type { TransactionSortType } from '@/types/TransactionSortTypes';
  import { ref, type PropType, computed } from 'vue'

  defineProps({
    transactions: {
      required: true,
      type: Array as PropType<ClientTransactionDTO[]>
    }
  })

  const sortField = ref<TransactionSortType>('transactionID');

  const sortedTransactions = computed(() => {
  return '';
  })
  
</script>

<style lang="scss" scoped>
  .scroll {
    height: 100%;
    overflow-y: auto;

    scrollbar-width: thin;
  }

  .title-row{
    display: flex;
    flex-direction: row;
    h2{
      margin: 0;
    }
  }

  .heading-row{
    display: flex;
    flex-direction: row;

    .heading-row-item{
      width: 30%;
      h3{
        text-align: center;
      }
    }

    .heading-row-end{
      width: 10%;
    }

  }
  
</style>
