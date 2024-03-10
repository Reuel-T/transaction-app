<template>
  <div class="transaction">
    <div class="info">
      <p>
        {{
          localTransaction.amount.toLocaleString('en-ZA', { style: 'currency', currency: 'ZAR' })
        }}
      </p>
    </div>
    <div class="info">
      <p>{{ localTransaction.transactionTypeName }}</p>
    </div>
    <div class="info">
      <p>{{ localTransaction.comment }}</p>
    </div>
    <div class="info">
      <form
        @submit.prevent="
          () => {
            handleSubmit()
          }
        "
      >
        <input
          v-model="formData.comment"
          type="text"
          placeholder="Transaction Comment"
          :disabled="isUpdating"
        />
        <button
          :type="'submit'"
          :disabled="isUpdating"
        >
          Update Comment
        </button>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
  import type { ClientTransactionDTO } from '@/models/ClientTransactionDTO'
  import { usePutTransactionComment } from '@/shared/usePutComment'
  import { ref, type PropType, watch, reactive } from 'vue'

  const props = defineProps({
    transaction: {
      required: true,
      type: Object as PropType<ClientTransactionDTO>
    }
  })

  //set the transaction ID if update is needed
  const formData = reactive({
    transactionID: props.transaction.transactionID,
    comment: ''
  })

  /*
    create a transaction scoped locally to the component based on the one 
    passed in through props
  */
  const localTransaction = ref({ ...props.transaction })

  /* 
    watch for changes in the passed in prop.
    if the passed in prop changes, set the locally scoped
    transaction to the updated one
  */
  watch(props.transaction, (newValue) => {
    localTransaction.value = newValue
  })

  /* 
    When the post is successful, set the locally scoped
    transaction to the one passed in from the API
  */
  function commentUpdated(data: ClientTransactionDTO) {
    localTransaction.value.comment = data.comment
    formData.comment = ''
  }

  /* 
    just a hook for managing these comment updates
  */
  const { isUpdating, putData } = usePutTransactionComment({ onSuccess: commentUpdated })

  /* 
    runs when the user submits the form
  */
  function handleSubmit() {
    putData(formData)
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
