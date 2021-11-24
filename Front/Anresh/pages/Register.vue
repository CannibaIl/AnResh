<template>
    <section class="empty-section">
        <register-form class="form-wrap"/>
    </section>
</template>

<script>
import RegisterForm from '../components/Auth/RegisterForm.vue'

export default {
  components: { RegisterForm },
    layout: 'empty'
}
</script>
