<template>
  <div class="transaction">
    <div class="info">
      <p>
        {{ transaction.amount.toLocaleString('en-ZA', { style: 'currency', currency: 'ZAR' }) }}
      </p>
    </div>
    <div class="info">
      <p>{{ transaction.transactionTypeName }}</p>
    </div>
    <div class="info">
      <p>{{ transaction.comment }}</p>
    </div>
  </div>
</template>

<script setup lang="ts">
  import type { ClientTransactionDTO } from '@/models/ClientTransactionDTO'
  import type { PropType } from 'vue'

  const props = defineProps({
    transaction: {
      required: true,
      type: Object as PropType<ClientTransactionDTO>
    }
  })
</script>

<style lang="scss" scoped>
  .transaction {
    display: flex;
    flex-direction: row;

    .info {
      width: 10rem;
    }
  }
</style>
@/models/ClientTransactionDTO
