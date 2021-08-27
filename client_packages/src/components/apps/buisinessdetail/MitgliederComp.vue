<template id="Mitglieder">
    <div style="width:100%">
        <div class="shadow_businessdetail">
            <v-ons-row class="fRow_businessdetail">
                <v-ons-col>
                    Businesskonto: ${{new Intl.NumberFormat().format(secdata)}}
                </v-ons-col>
            </v-ons-row>
        </div>
            <div class="shadow_businessdetail">
                <v-ons-row class="fRow_businessdetail">
                    <v-ons-col>
                        <table id="customers">
                            <tr><th>Name</th><th>Gehalt</th></tr>
                            <tr v-for="member in data.BusinessMemberList" :key="member.name">
                                <td>{{member.name}}</td>
                                <td>{{member.salary}}</td>
                            </tr>
                        </table>
                    </v-ons-col>
                </v-ons-row>
            </div>
        </div>
</template>

<script>

  export default {
        template: "#Mitglieder",
        data() {
            return {
                anzMember: 0,
            }
        },
        mounted() {
        },
        props: ['data','secdata']
  };
</script>

<style lang="scss" scoped>
    section {
        margin: 20px;
    }

    .fRow {
        padding: 1rem 0 0 1rem;
    }

    .mittig {
        text-align: center;
    }

    .pointerClass {
        cursor: pointer;
    }
    #customers {
        font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
        border-collapse: collapse;
        width: 100%;
    }

        #customers td, #customers th {
            border: 1px solid #ddd;
            padding: 8px;
        }

        #customers tr:nth-child(even) {
            background-color: #f2f2f2;
        }

        #customers th {
            padding-top: 12px;
            padding-bottom: 12px;
            text-align: left;
            background-color: lightblue;
            color: white;
        }
  
</style>