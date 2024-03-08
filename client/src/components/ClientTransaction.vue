<template>
  <div class="transaction">
    <div class="info">
      <p>
        {{ localTransaction.amount.toLocaleString('en-ZA', { style: 'currency', currency: 'ZAR' }) }}
      </p>
    </div>
    <div class="info">
      <p>{{ localTransaction.transactionTypeName }}</p>
    </div>
    <div class="info">
      <p>{{ localTransaction.comment }}</p>
    </div>
    <div class="info">
      <form @submit.prevent="() => {handleSubmit()}">
        <input v-model="formData.comment" type="text" placeholder="Transaction Comment" :disabled="isUpdating"/>
        <button :disabled="isUpdating">Update Comment</button>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
  import type { ClientTransactionDTO } from '@/models/ClientTransactionDTO'
import { usePutTransactionComment } from '@/shared/usePutComment';
  import { ref, type PropType, watch, reactive } from 'vue'
  
  const props = defineProps({
    transaction: {
      required: true,
      type: Object as PropType<ClientTransactionDTO>
    }
  });

  const formData = reactive({
    transactionID: props.transaction.transactionID,
    comment: ''
  });

  const localTransaction = ref({ ...props.transaction })

  watch(props.transaction, (newValue) => {
    localTransaction.value = newValue;
  })

  function commentUpdated(data: ClientTransactionDTO) {
    localTransaction.value.comment = data.comment;  
    formData.comment = '';
  }

  const { isUpdating, putData} = usePutTransactionComment({onSuccess: commentUpdated})

  function handleSubmit() {
    putData(formData);
  }

</script>

<style lang="scss" scoped>
  .transaction {
    display: flex;
    flex-direction: row;

    .info {
      width: 20rem;
    }
  }
</style>
