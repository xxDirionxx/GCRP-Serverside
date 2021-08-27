<template>
    <div>
        <div class="jumbotron center card">
            <h4 id="title">{{ confirmData.confirmationObject.Title }}</h4>
            <h6>
                {{ confirmData.confirmationObject.Message }}
            </h6>
            <div class="form">
                <button class="btn" v-on:click="handle">Bestätigen</button>
                
            </div>
        </div>
    </div>
</template>

<script>
    import Windows from "../windows"

    export default Windows.register({
        name: "Confirmation",
        props: {
            data: String
        },
        data() {
            return {
                confirmData: JSON.parse(this.data)
            }
        },
        methods: {
            dismiss() {
                this.dismiss()
            },
            handle() {
                this.triggerServer(this.confirmData.confirmationObject.Callback, this.confirmData.confirmationObject.Arg1, this.confirmData.confirmationObject.Arg2)
                this.dismiss()
            }
        }
    })
</script>

<style lang="scss" scoped>
</style>